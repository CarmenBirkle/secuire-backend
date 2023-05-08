namespace PWManagerService.Model
{
    public abstract class DataEntry
    {
        public int Id { get; set; } 
        public string Subject { get; set; }
        public Dictionary<string, string> CustomTopics { get; set; }
        public string Comment { get; set; }
        public bool Favourite { get; set; }
    }
}
