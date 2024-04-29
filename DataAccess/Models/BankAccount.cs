using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class BankAccount
    {
        public BankAccount()
        {

        }

        public int Id { get; set; }

        public string BankName { get; set; } // Ejemplo: BCP, Interbank

        public string AccountType { get; set; } // Ejemplo: Ahorros, Corriente

        public string AccountNumber { get; set; }

        public string CCI { get; set; } // Código de cuenta interbancario

        public int UserInformationId { get; set; }

        public virtual UserInformation UserInformation { get; set; }
    }
}