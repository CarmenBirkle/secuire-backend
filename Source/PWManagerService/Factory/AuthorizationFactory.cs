using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public AuthorizationFactory(DataContext dataContext, UserManager<IdentityUser> userManager, ILogger logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.userManager = userManager;
        }


        public async Task<(User?, int)> UpdateAccount(string jwtToken, RegistrationData updatedUser)
        {
            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);
            if (user == null) return (null, 400);

            Task<IdentityResult> taskChangeMail = null;
            Task<IdentityResult> taskChangeUsername = null;
            Task<IdentityResult> taskChangePassword = null;

            IdentityUser dummyUser = new IdentityUser();
            dummyUser.PasswordHash = updatedUser.HashedPassword;

            // ToDo: Passwort irgendwie sauber abgleichen.... oder einfach draufknallen??
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            string hashedPw = passwordHasher.HashPassword(dummyUser, dummyUser.PasswordHash);


            return (null, 0);


            if (user.IdentityUser.Email != updatedUser.Email)
                taskChangeMail = userManager.ChangeEmailAsync(user.IdentityUser, updatedUser.Email, jwtToken);
            if (user.IdentityUser.PasswordHash != updatedUser.HashedPassword)
            {
                taskChangePassword = userManager.ChangePasswordAsync(user.IdentityUser, user.IdentityUser.PasswordHash, updatedUser.HashedPassword);

            }
            if (user.IdentityUser.UserName != updatedUser.Username)
                taskChangeUsername = userManager.SetUserNameAsync(user.IdentityUser, updatedUser.Username);
            user.AgbAcceptedAt = updatedUser.AgbAcceptedAt;
            user.PasswordHint = updatedUser.PasswordHint;
            user.Salt = updatedUser.Salt;

            if (taskChangeMail != null) taskChangeMail.Wait();
            if (taskChangeUsername != null) taskChangeUsername.Wait();
            if (taskChangePassword != null) taskChangePassword.Wait();

            dataContext.SaveChanges();

            return (user, 200);

        }

        public async Task<int> DeleteAccount(string jwtToken)
        {
            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);
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
