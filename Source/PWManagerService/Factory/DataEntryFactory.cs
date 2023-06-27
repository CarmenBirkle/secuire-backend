using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using PWManagerService.Controllers;
using PWManagerServiceModelEF;
using System.Diagnostics.Eventing.Reader;

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


        #region Get
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

        private async Task<EntryType?> ReadCategory(string category)
        {
            object? enumCategory = EntryType.UNDEFINED;
            if (Enum.TryParse(typeof(EntryType), category.ToUpper(), out enumCategory))
                return enumCategory as EntryType?;
            else
                return null;
                
        }
        #endregion

        #region Delete

        public async Task<int> DeleteDataEntry(string jwtToken, int entryId)
        {
            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);

            (object?, DataEntry?) fullEntry = dataContext.GetFullEntry(entryId, user.IdentityUserId);
            if (fullEntry.Item1 == null || fullEntry.Item2 == null)
                return 404;

            Type childEntryType = fullEntry.Item1.GetType();
            if (childEntryType == typeof(Login))
                dataContext.Login.Remove((Login)fullEntry.Item1);
            else if(childEntryType == typeof(SafeNote))
                dataContext.SafeNote.Remove((SafeNote)fullEntry.Item1);
            else if (childEntryType == typeof(PaymentCard))
                dataContext.PaymentCard.Remove((PaymentCard)fullEntry.Item1);

            dataContext.DataEntry.Remove((DataEntry)fullEntry.Item2);

            dataContext.SaveChanges();

            return 200;
        }

        #endregion

        #region Update
        public async Task<(int, object?)> UpdateDataEntry(int id, DataEntryClientRequest requestData, string jwtToken)
        {
            EntryType? category = await ReadCategory(requestData.Category??"");
            if (category == null)
                return (400, null);

            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);

            DataEntry? entry = dataContext.GetDataEntry(id, user.IdentityUserId);
            if (entry == null)
                return (404, null);
            else
            {
                entry.Favourite = requestData.Favourite ?? "";
                entry.CustomTopics = requestData.CustomTopics ?? "";
                entry.Comment = requestData.Comment ?? "";
                entry.SelectedIcon = requestData.SelectedIcon ?? "";
                entry.Subject = requestData.Subject ?? "";
            }
            

            object? updatedData = null;

            switch(category)
            {
                case EntryType.PAYMENTCARD:
                    updatedData = await UpdatePaymentCard(id, requestData, entry);
                    break;

                case EntryType.SAFENOTE:
                    updatedData = await UpdateSafeNote(id, requestData, entry);
                    break;

                case EntryType.LOGIN:
                    updatedData = await UpdateLogin(id, requestData, entry);
                    break;

                default:
                    return (400, null);
            }

            dataContext.SaveChanges();
            return (200,  updatedData);
        }


        public async Task<SafeNote?> UpdateSafeNote(int id, DataEntryClientRequest requestData, DataEntry entry)
        {
            SafeNote safeNote = dataContext.GetSafeNote(id);

            if (safeNote == null) return null;

            safeNote.DataEntry = entry;
            safeNote.DataEntryId = entry.Id;
            safeNote.Note = requestData.Note??"";

            return safeNote;
        }


        public async Task<Login?> UpdateLogin(int id, DataEntryClientRequest requestData, DataEntry entry)
        {
            Login login = dataContext.GetLogin(id);

            if (login == null) return null;

            login.DataEntry = entry;
            login.DataEntryId = entry.Id;

            login.Username = requestData.Username ?? "";
            login.Password = requestData.Password ?? "";
            login.Url = requestData.Url ?? "";

            return login;
        }

        public async Task<PaymentCard?> UpdatePaymentCard(int id, DataEntryClientRequest requestData, DataEntry entry)
        {
            PaymentCard paymentCard = dataContext.GetPaymentCard(id);
            if (paymentCard == null) return null;

            paymentCard.DataEntry = entry;
            paymentCard.DataEntryId = entry.Id;

            paymentCard.Owner = requestData.Owner ?? "";
            paymentCard.Number = requestData.CardNumber ?? "";
            paymentCard.ExpirationDate = requestData.ExpirationDate ?? "";
            paymentCard.Pin = requestData.Pin ?? "";
            paymentCard.Cvv = requestData.Cvv ?? "";
            paymentCard.CardType = requestData.CardType ?? "";

            return paymentCard;
        }
        #endregion

        #region Create

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(int, object?)> CreateDataEntry(string jwtToken, DataEntryClientRequest requestData)
        {
            EntryType? category = await ReadCategory(requestData.Category ?? "");
            if (category == null)
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

        #endregion

    }
}
