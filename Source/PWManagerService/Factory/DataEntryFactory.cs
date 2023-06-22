using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
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
        /// <param name="dataEntryClientRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(int, object?)> CreateDataEntry(string jwtToken, DataEntryClientRequest dataEntryClientRequest)
        {
            object? category = EntryType.UNDEFINED;
            if (!Enum.TryParse(typeof(EntryType), (dataEntryClientRequest.Category ?? ""), out category))
                return (400, null);

            User user = await dataContext.GetUser(TokenService.GetUserMail(jwtToken), userManager);

            DataEntry entry = new DataEntry
            {
                UserId = user.IdentityUserId,
                Comment = dataEntryClientRequest.Comment ?? "",
                Favourite = dataEntryClientRequest.Favourite ?? "",
                Subject = dataEntryClientRequest.Subject ?? "",
                CustomTopics = dataEntryClientRequest.CustomTopics ?? "",
                SelectedIcon = dataEntryClientRequest.SelectedIcon ?? ""
            };

            await dataContext.DataEntry.AddAsync(entry);
            dataContext.SaveChanges();

            switch (category)
            {
                case EntryType.LOGIN:

                    break;

                case EntryType.SAFENOTE:
                    {
                        SafeNote safeNote = await CreateSafeNote(dataEntryClientRequest);
                        safeNote.DataEntry = entry;
                        user.DataEntries.Add(entry);
                        


                        return (201, safeNote);
                    }
                case EntryType.PAYMENTCARD:

                    break;

                default:
                    return (400, null);
            }

            return (500, null);
        }

        private async Task<Login> CreateLogin(DataEntryClientRequest request)
        {
            return null;
        }

        private async Task<SafeNote> CreateSafeNote(DataEntryClientRequest request)
        {
            SafeNote safeNote = new SafeNote();
            safeNote.Note = request.Note ?? "";

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
