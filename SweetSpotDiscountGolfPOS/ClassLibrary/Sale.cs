using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //Used to define and create a sale
    public class Sale
    {
        public int invoiceId { get; set; }
        public int sku { get; set; }
        public int quantity { get; set; }

        public Sale() { }

        public Sale(int InvoiceID, int SKU, int Quantity)
        {
            invoiceId = InvoiceID;
            sku = SKU;
            quantity = Quantity;
        }

    }
}
