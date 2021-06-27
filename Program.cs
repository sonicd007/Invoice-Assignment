using System;
using System.Collections.Generic;
using System.Linq;
namespace bread2
{
    class Program
    {
        static void Main(string[] args)
        {
            Subscription newPlan = new Subscription(1, 1, 4);

            User[] noUsers = new User[0];

            User[] constantUsers = {
              new User(1, "Employee #1", new DateTime(2018, 11, 4), DateTime.MaxValue, 1),
              new User(2, "Employee #2", new DateTime(2018, 12, 4), DateTime.MaxValue, 1)
          };

                    User[] userSignedUp = {
              new User(1, "Employee #1", new DateTime(2018, 11, 4), DateTime.MaxValue, 1),
              new User(2, "Employee #2", new DateTime(2018, 12, 4), DateTime.MaxValue, 1),
              new User(3, "Employee #3", new DateTime(2019, 1, 10), DateTime.MaxValue, 1),
          };


            Console.WriteLine(Challenge.BillFor("2019-01", newPlan, constantUsers));
        }
    }

    public class Challenge
    {
        // Dictionaries are being used for faster lookup
        static Dictionary<int, Dictionary<string, Invoice>> customerInvoicesDict = new Dictionary<int, Dictionary<string, Invoice>>();
        public static Decimal BillFor(string month, Subscription activeSubscription, User[] users)
        {
            DateTime billingMonth = Convert.ToDateTime(month);

            if (null != activeSubscription)
            {
                InitializeCustomerInvoiceMappings(month, activeSubscription, billingMonth);

                customerInvoicesDict[activeSubscription.CustomerId][month].AddSubscription(activeSubscription);
            }

            if (users.Length > 0)
            {
                foreach (User user in users)
                {
                    InitializeCustomerInvoiceMappings(month, user, billingMonth);

                    customerInvoicesDict[user.CustomerId][month].AddUser(user);
                }
            }

            // I was expecting this to return a single customers monthly bill but the function parameters allow for cases where multiple customers can come in through this so I'm returning the aggregate of all customers billed for this month
            // This function should take in a customer id parameter to specify which customer in particular is being expected to be billed

            decimal thisMonthsTotalBills = 0.00m;
            foreach(KeyValuePair<int, Dictionary<string, Invoice>> customerEntry in customerInvoicesDict)
            {
                thisMonthsTotalBills += customerEntry.Value[month].RunningTotal;
            }

            return thisMonthsTotalBills;
        }

        private static void InitializeCustomerInvoiceMappings(string month, Subscription activeSubscription, DateTime billingMonth)
        {
            if (!customerInvoicesDict.ContainsKey(activeSubscription.CustomerId))
            {
                customerInvoicesDict.Add(activeSubscription.CustomerId, new Dictionary<string, Invoice>());
            }

            if (!customerInvoicesDict[activeSubscription.CustomerId].ContainsKey(month))
            {
                customerInvoicesDict[activeSubscription.CustomerId].Add(month, new Invoice(billingMonth));
            }
        }
        private static void InitializeCustomerInvoiceMappings(string month, User user, DateTime billingMonth)
        {
            if (!customerInvoicesDict.ContainsKey(user.CustomerId))
            {
                customerInvoicesDict.Add(user.CustomerId, new Dictionary<string, Invoice>());
            }

            if (!customerInvoicesDict[user.CustomerId].ContainsKey(month))
            {
                customerInvoicesDict[user.CustomerId].Add(month, new Invoice(billingMonth));
            }
        }
    }
}
