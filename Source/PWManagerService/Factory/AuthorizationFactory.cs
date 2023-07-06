using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;
using PWManagerService.Model;
using PWManagerServiceModelEF;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

namespace PWManagerService
{
    public class AuthorizationFactory
    {
        private DataContext dataContext;
        private ILogger logger;
        private UserManager<IdentityUser> userManager;
        private TokenService tokenService;
        public AuthorizationFactory(DataContext dataContext, UserManager<IdentityUser> userManager, ILogger logger, TokenService tokenService)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<(User?, int)> Login(AuthentificationData loginData)
        {
            User? user = await dataContext.GetUser(loginData.Email, userManager);

            if(user == null)
                return (null, 400);

            List<UserFailedLoginHistory> failedLoginHistory = dataContext.GetUserFailedLoginHistory(user);
            List<UserFailedLoginHistory> relevantFailedLoginHistory = failedLoginHistory.Where(h => h.TimeStamp.AddSeconds((double)Appsettings.Instance.BlockUserTimespanInSec) >= DateTime.Now).ToList();
            if (relevantFailedLoginHistory.Count >= Appsettings.Instance.BlockUserTries)
                return (null, 403);


            bool isPasswordValid = await userManager.CheckPasswordAsync(user.IdentityUser, loginData.HashedPassword);
            if(!isPasswordValid)
            {
                UserFailedLoginHistory historyEntry = new UserFailedLoginHistory();
                historyEntry.TimeStamp = DateTime.Now;
                historyEntry.UserId = user.IdentityUser.Id;
                dataContext.UserFailedLoginHistories.Add(historyEntry);
                await dataContext.SaveChangesAsync();

                return (null, 400);
            }

            user.JwtToken = tokenService.CreateToken(user.IdentityUser);
            await dataContext.SaveChangesAsync();

            user.DataEntries = dataContext.GetDataEntry(user.IdentityUserId);
            user.FailedLogins = failedLoginHistory.Count;
            dataContext.DeleteUserFailedLoginHistory(user);
            dataContext.SaveChanges();
            return (user, 200);

        }

        public async Task<(User?, int)> UpdateAccount(string jwtToken, AccountPostPutData updatedUser)
        {
            User? user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);
            if (user == null) return (null, 400);

            //List<Task<IdentityResult>> tasks = new List<Task<IdentityResult>>();
            string oldUsername = string.Empty;
            string oldMail = string.Empty;


            bool tasksSucceeded = true;
            if (user.IdentityUser.Email != updatedUser.Email)
            {
                oldMail = user.IdentityUser.Email;
                string emailToken = await userManager.GenerateChangeEmailTokenAsync(user.IdentityUser, updatedUser.Email);
                Task<IdentityResult> task = userManager.ChangeEmailAsync(user.IdentityUser, updatedUser.Email, emailToken);
                task.Wait();
                if (!task.Result.Succeeded)
                    tasksSucceeded = false;
            }
            if (!string.IsNullOrEmpty(updatedUser.NewHashedPassword))
            {
                Task<IdentityResult> task = userManager.ChangePasswordAsync(user.IdentityUser, updatedUser.HashedPassword, updatedUser.NewHashedPassword);
                task.Wait();
                if (!task.Result.Succeeded)
                    tasksSucceeded = false;
            }

            if (user.IdentityUser.UserName != updatedUser.Username)
            {
                oldUsername = user.IdentityUser.UserName??"";
                Task<IdentityResult> task = userManager.SetUserNameAsync(user.IdentityUser, updatedUser.Username);
                task.Wait();
                if (!task.Result.Succeeded)
                    tasksSucceeded = false;
            }
            user.AgbAcceptedAt = updatedUser.AgbAcceptedAt;
            user.PasswordHint = updatedUser.PasswordHint;
            user.Salt = updatedUser.Salt;

            //tasks.ForEach(
            //    task =>
            //    {
            //        task.Wait();
            //        if (!task.Result.Succeeded) tasksSucceeded = false;
            //    });

            if (!tasksSucceeded)
            {
                //Rollback
                if (!string.IsNullOrEmpty(oldUsername))
                    await userManager.SetUserNameAsync(user.IdentityUser, oldUsername);
                if (!string.IsNullOrEmpty(oldMail))
                {
                    string emailToken = await userManager.GenerateChangeEmailTokenAsync(user.IdentityUser, oldMail);
                    await userManager.ChangeEmailAsync(user.IdentityUser, oldMail, emailToken);
                }
                if (!string.IsNullOrEmpty(updatedUser.NewHashedPassword))
                    await userManager.ChangePasswordAsync(user.IdentityUser, updatedUser.NewHashedPassword, updatedUser.HashedPassword);
                return (null, 500);
            }
            else
            {
                user.JwtToken = tokenService.CreateToken(user.IdentityUser);
                dataContext.SaveChanges();
                return (user, 200);
            }

        }

        public async Task<int> DeleteAccount(string jwtToken)
        {
            User? user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);
            if (user == null)
                return 404;

            List<object> childEntries = dataContext.GetAllDataEntries(user);

            foreach (object childEntry in childEntries)
            {
                Type childType = childEntry.GetType();
                if (childType == typeof(Login))
                {
                    dataContext.Login.Remove((Login)childEntry);
                    dataContext.DataEntry.Remove(((Login)childEntry).DataEntry);
                }
                if (childType == typeof(SafeNote))
                {
                    dataContext.SafeNote.Remove((SafeNote)childEntry);
                    dataContext.DataEntry.Remove(((SafeNote)childEntry).DataEntry);
                }
                if (childType == typeof(PaymentCard))
                {
                    dataContext.PaymentCard.Remove((PaymentCard)childEntry);
                    dataContext.DataEntry.Remove(((PaymentCard)childEntry).DataEntry);
                }
            }

            dataContext.User.Remove(user);
            Task deleteIdentityUser = userManager.DeleteAsync(user.IdentityUser);
            deleteIdentityUser.Wait();

            dataContext.SaveChanges();
            return 200;

        }

        public async Task<int> LogOut(string jwtToken)
        {
            throw new NotImplementedException();
        }
    }
}
