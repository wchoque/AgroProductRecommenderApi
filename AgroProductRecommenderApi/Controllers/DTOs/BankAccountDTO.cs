namespace AgroProductRecommenderApi.Controllers.DTOs
{
    public class BankAccountDTO
    {
        public int Id { get; set; }

        public string BankName { get; set; }

        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public string CCI { get; set; }

        public int UserId { get; set; }
    }
}
