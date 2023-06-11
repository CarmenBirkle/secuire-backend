using PWManagerServiceModelEF;
using System.ComponentModel;

namespace PWManagerService
{
    public class DataEntryClientRequest
    {
        public DataEntryClientRequest() { }

        #region General

        [DefaultValue("test kategorie")]
        public string? Category { get; set; }
        public string? Subject { get; set; }
        //public List<CustomTopic>? CustomTopics { get; set; }
        public bool Favourite { get; set; }
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

        public string? Owner { get; set; }
        public string? CardNumber { get; set; }
        public int? CardTypeId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Pin { get; set; }
        public string? Cvv { get; set; }

        #endregion
    }


}
