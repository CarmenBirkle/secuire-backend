using PWManagerServiceModelEF;
using System.ComponentModel;

namespace PWManagerService
{
    public class DataEntryClientRequest
    {
        public DataEntryClientRequest() { }

        #region General
        public string? Category { get; set; }
        public string? Subject { get; set; }
        public string? CustomTopics { get; set; }
        public string? Favourite { get; set; }
        public string? Comment { get; set; }
        public string? SelectedIcon { get; set; }
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
        public string? ExpirationDate { get; set; }
        public string? Pin { get; set; }
        public string? Cvv { get; set; }

        #endregion
    }


}
