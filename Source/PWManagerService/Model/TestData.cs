using PWManagerServiceModelEF;

namespace PWManagerService.Model
{
    public class TestData
    {
        public static Random random = new();

        private static readonly List<int> userIds = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static readonly List<string> subjects = new List<string> { "Subject 1", "Subject 2", "Subject 3", "Subject 4", "Subject 5" };
        private static readonly List<bool> favourite = new List<bool> { true,false };
        private static readonly List<string> comments = new List<string> { "Comment 1", "Comment 2", "Comment 3", "Comment 4", "Comment 5" };
        // Custom Topic String

        private static readonly List<string> notes = new List<string> { "Note 1", "Note 2", "Note 3", "Note 4", "Note 5" };

        //public static List<object> CreateRandomSafeNote()
        //{
        //    DataEntry dataEntry = new DataEntry()
        //    {
        //        UserId = userIds.ElementAt(random.Next(0,userIds.Count)),
        //        Subject = subjects.ElementAt(random.Next(0,subjects.Count)),
        //        Favourite = favourite.ElementAt(random.Next(0,favourite.Count)),
        //        Comment = comments.ElementAt(random.Next(0,comments.Count))
        //    };

        //    SafeNote safeNote = new SafeNote()
        //    {
        //        Note = notes.ElementAt(random.Next(0, notes.Count))
        //    };

        //    return new List<object> { dataEntry, safeNote };
        //}

    }
}
