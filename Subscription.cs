using System;
using System.Collections.Generic;
using System.Text;

namespace bread2
{
    public class Subscription
    {
        public Subscription() { }
        public Subscription(int id, int customerId, int monthlyPriceInDollars)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.MonthlyPriceInDollars = monthlyPriceInDollars;
        }

        public int Id;
        public int CustomerId;
        public int MonthlyPriceInDollars;
    }
}
