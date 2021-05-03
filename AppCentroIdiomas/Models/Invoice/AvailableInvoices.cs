using System.Collections;
using System.Collections.Generic;

namespace AppCentroIdiomas.Models
{
    public class AvailableInvoices : IEnumerable<AvailableInvoice> {
        public List<AvailableInvoice> AvailableInvoicesList = new List<AvailableInvoice>();
        public AvailableInvoice this[int index]
        {
            get { return AvailableInvoicesList[index]; }
            set { AvailableInvoicesList.Insert(index, value); }
        }

        public IEnumerator<AvailableInvoice> GetEnumerator()
        {
            return AvailableInvoicesList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}