using PWManagerServiceModelEF;

namespace PWManagerService
{
    public class DataEntryClientRequest
    {
        public DataEntryClientRequest() { }

        #region General

        public int? Id { get; set; }
        public string? Category { get; set; }
        public string? Subject { get; set; }
        //public List<CustomTopic>? CustomTopics { get; set; }
        public string Favourite { get; set; }
        public string? Comment { get; set; }
        #endregion

        #region Login
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Url { get; set; }
        #endregion

        #region Safe Note
        public string? Note { get; set; }
        #endregion

        #region Payment Card
        public string? Pin { get; set; }
        public string? Cardnumber { get; set; }
        public string? Expirationdate { get; set; }
        public string? Owner { get; set; }
        public string? Cvv { get; set; }
        public string? Cardtype { get; set; }
        #endregion
    }

  
}
