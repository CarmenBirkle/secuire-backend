using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using PWManagerServiceModelEF;
using System.ComponentModel;
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
