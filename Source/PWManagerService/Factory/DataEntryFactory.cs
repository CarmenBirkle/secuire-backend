using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using PWManagerService.Controllers;
using PWManagerServiceModelEF;

namespace PWManagerService.Factory
{
    public class DataEntryFactory
    {
        private DataContext dataContext;
        private ILogger logger;
        private UserManager<IdentityUser> userManager;
        public DataEntryFactory(DataContext dataContext, UserManager<IdentityUser> userManager, ILogger logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.userManager = userManager;
        }

        /// <summary>
        /// Liefert alle Dataentry Eintraege eines Users zurueck
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<object>> GetAllDataEntries(string jwtToken, UserManager<IdentityUser> userManager)
        {
            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);
            List<object> dataEntries = dataContext.GetAllDataEntries(user);
            return dataEntries;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(int, object?)> CreateDataEntry(string jwtToken, DataEntryClientRequest requestData)
        {
            object? category = EntryType.UNDEFINED;
            if (!Enum.TryParse(typeof(EntryType), (requestData.Category ?? "").ToUpper(), out category))
                return (400, null);

            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);

            object savedData;

            switch (category)
            {
                case EntryType.LOGIN:
                        savedData = await CreateLogin(requestData, user.IdentityUserId);
                        break;

                case EntryType.SAFENOTE:
                        savedData = await CreateSafeNote(requestData, user.IdentityUserId);
                        break;

                case EntryType.PAYMENTCARD:
                        savedData = await CreatePaymentCard(requestData, user.IdentityUserId);
                        break;

                default:
                    return (400, null);
            }

            dataContext.SaveChanges();
            return (201, savedData);
        }

        private async Task<DataEntry> SaveAndCreateDataEntry(DataEntryClientRequest requestData, string userId)
        {
            DataEntry entry = new DataEntry
            {
                UserId = userId,
                Comment = requestData.Comment ?? "",
                Favourite = requestData.Favourite ?? "",
                Subject = requestData.Subject ?? "",
                CustomTopics = requestData.CustomTopics ?? "",
                SelectedIcon = requestData.SelectedIcon ?? ""
            };

            await dataContext.DataEntry.AddAsync(entry);
            dataContext.SaveChanges();

            return entry;
        }

        private async Task<PaymentCard> CreatePaymentCard(DataEntryClientRequest requestData, string userId)
        {
            DataEntry entry = await SaveAndCreateDataEntry(requestData, userId);

            PaymentCard paymentCard = new PaymentCard
            {
                DataEntry = entry,
                DataEntryId = entry.Id,
                CardType = requestData.CardType ?? "",
                Cvv = requestData.Cvv ?? "",
                ExpirationDate = requestData.ExpirationDate ?? "",
                Number = requestData.CardNumber ?? "",
                Owner = requestData.Owner ?? "",
                Pin = requestData.Pin ?? ""
            };

            await dataContext.PaymentCard.AddAsync(paymentCard);
            dataContext.SaveChanges();

            return paymentCard;
        }

        private async Task<Login> CreateLogin(DataEntryClientRequest requestData, string userId)
        {
            DataEntry entry = await SaveAndCreateDataEntry(requestData, userId);

            Login login = new Login
            {
                DataEntryId = entry.Id,
                Username = requestData.Username ?? "",
                Password = requestData.Password ?? "",
                Url = requestData.Url ?? ""
            };
            await dataContext.Login.AddAsync(login);
            dataContext.SaveChanges();

            return login;
        }

        private async Task<SafeNote> CreateSafeNote(DataEntryClientRequest requestData, string userId)
        {
            DataEntry entry = await SaveAndCreateDataEntry(requestData, userId);

            SafeNote safeNote = new SafeNote();
            safeNote.Note = requestData.Note ?? "";
            safeNote.DataEntryId = entry.Id;
            safeNote.DataEntry = entry;

            await dataContext.SafeNote.AddAsync(safeNote);

            return safeNote;
        }


        /// <summary>
        /// liest JWT Token aus Header
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string ReadToken(IHeaderDictionary headers)
        {
            return headers.Authorization.ToString().Replace("Bearer ", "");
        }
    }
}
