using System;

namespace AppCentroIdiomas.Models
{
    public class AvailableInvoice
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Semester { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public string Deadline { get; set; }
        public string PaidAt { get; set; }
    }
}
