using PWManagerServiceModelEF;

namespace PWManagerService.Factory
{
    public static class DataEntryFactory
    {
        public static DataEntry InitEntry(DataEntryClientRequest clientData, out PostResponseBody<DataEntry> body, ILogger logger)
        {
            body = new PostResponseBody<DataEntry>();
            EntryType category;
            DataEntry entry;

            if (!Enum.TryParse(clientData.Category.ToUpper(), out category))
            {
                body.StatusCode = System.Net.HttpStatusCode.BadRequest;
                body.ResponseMessage = "Missing or Wrong Category";
                logger.LogWarning("Missing or Wrong Category");

                return null;
            }

            switch (category)
            {
                case EntryType.SAFENOTE:
                    entry = new SafeNoteEntry();
                    break;

                case EntryType.PAYMENTCARD:
                    entry = new PaymentCardEntry();
                    break;

                case EntryType.LOGIN:
                    entry = new LoginEntry();
                    break;

                default:
                    body.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    body.ResponseMessage = "Missing or Wrong Category";
                    logger.LogWarning("Missing or Wrong Category");
                    return null;
            }
            body.StatusCode = System.Net.HttpStatusCode.OK;
            entry.FillData(clientData, ref body, logger);
            return entry;
        }

        /// <summary>
        /// initialisiert die Felder
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="clientData"></param>
        /// <param name="body"></param>
        /// <param name="logger"></param>
        private static void FillData(this DataEntry entry, DataEntryClientRequest clientData, ref PostResponseBody<DataEntry> body, ILogger logger)
        {
            entry.Id = clientData.Id;
            entry.Subject = clientData.Subject;
            entry.CustomTopics = clientData.CustomTopics;
            entry.Comment = clientData.Comment;
            entry.Favourite = clientData.Favourite;

            if(entry.GetType() == typeof(LoginEntry))
            {
                LoginEntry lEntry = (LoginEntry)entry;
                lEntry.Username = clientData.Username;
                lEntry.Password = clientData.Password;
                lEntry.Url = clientData.Url;
            }
            if(entry.GetType() == typeof(PaymentCardEntry))
            {
                PaymentCardEntry pcEntry = (PaymentCardEntry)entry;
                pcEntry.Owner = clientData.Owner;
                pcEntry.Cardnumber = clientData.Cardnumber;
                pcEntry.CardType = clientData.Cardtype;
                pcEntry.ExpirationDate = clientData.Expirationdate;
                pcEntry.Pin = clientData.Pin;
                pcEntry.Cvv = clientData.Cvv;
            }
            if(entry.GetType() == typeof(SafeNoteEntry))
            {
                SafeNoteEntry snEntry = (SafeNoteEntry)entry;
                snEntry.SafeNote = clientData.Note;
            }
        }

        public static void InsertEntry(this DataEntry entry, out PostResponseBody<DataEntry> body, ILogger logger, IDataHandler<DataEntry> dataHandler)
        {
            body = new PostResponseBody<DataEntry>();
            //body.RessourceLocation = dataHandler.InsertData(entry);
            entry.Id = 1;
            body.RessourceLocation = "api/DataEntry/Id/1";
            body.StatusCode = System.Net.HttpStatusCode.Created;
            body.Data = entry;
        }
    }
}
