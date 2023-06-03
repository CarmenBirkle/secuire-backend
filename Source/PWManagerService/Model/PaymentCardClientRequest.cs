namespace PWManagerService.Model
{
    public class PaymentCardClientRequest
    {
        public PaymentCardClientRequest() { }

        //public int? Id { get; set; }
        public string? Category { get; set; }
        public string? Subject { get; set; }
        public bool Favourite { get; set; }
        public string? Comment { get; set; }
        public string? Owner { get; set; }
        public string? Number { get; set; }
        public int CardTypeId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Pin { get; set; }
        public string? Cvv { get; set; }
    }
}
