using System;
using System.Collections.Generic;
using System.Text;

namespace Paycompute.Services.Implementation
{
    public class TaxService : ITaxService
    {
        private decimal taxRate;
        private decimal tax;

        public decimal TaxAmount(decimal totalAmount)
        {
            if (totalAmount <= 1042)
            {
                taxRate = 0m;
                tax = 0m;
            }
            else if (totalAmount > 1042 && totalAmount <= 3125)
            {
                taxRate = 0.2m;
                tax = (totalAmount - 1042) * taxRate;
            }
            else if (totalAmount > 3125 && totalAmount <= 12500)
            {
                taxRate = 0.4m;
                tax = (3125 - 1042) * 0.2m + (totalAmount - 3125) * taxRate;
            }
            else if (totalAmount > 12500)
            {
                taxRate = 0.45m;
                tax = (3125 - 1042) * 0.2m + (12500 - 3125) * 0.4m + (totalAmount - 12500) * taxRate;
            }
            return tax;
        }
    }
}
