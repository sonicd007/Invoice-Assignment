using System;
using System.Collections.Generic;
using System.Text;

namespace bread2
{
    public class Invoice
    {
        // The customerId this invoice belongs to
        private int customerId;
        // Key is subscriptionId
        private Dictionary<int,Subscription> subscriptions;
        // Key is userId
        private Dictionary<int,User> users;
        private decimal runningTotal;
        private DateTime billingPeriod;

        public Invoice(DateTime billingPeriod)
        {
            this.billingPeriod = billingPeriod;
            users = new Dictionary<int, User>();
            subscriptions = new Dictionary<int, Subscription>();
        }

        public int CustomerId { get => customerId; set => customerId = value; }

        public decimal RunningTotal { get => runningTotal; set => runningTotal = value; }
        public DateTime BillingPeriod { get => billingPeriod; set => billingPeriod = value; }

        public void AddUser(User user)
        {
            this.users.Add(user.Id, user);
            CalculateRunningTotal();
        }

        public void AddSubscription(Subscription subscription)
        {
            this.subscriptions.Add(subscription.Id, subscription);
            CalculateRunningTotal();
        }

        public void RemoveUser(int userId)
        {
            if (this.users.ContainsKey(userId))
            {
                this.users.Remove(userId);
                CalculateRunningTotal();
            }

            return;
        }

        public void RemoveSubscription(int subscriptionId)
        {
            if (this.subscriptions.ContainsKey(subscriptionId))
            {
                this.subscriptions.Remove(subscriptionId);
                CalculateRunningTotal();
            }
        }

        void CalculateRunningTotal()
        {
            // reset the value since we're about to set it here
            runningTotal = 0.00m;
            decimal monthlyCost = 0.00m;

            foreach (Subscription subscription in subscriptions.Values)
            {
                decimal subPriceForMonth = Convert.ToDecimal(subscription.MonthlyPriceInDollars);

                // No point in continuing because it's free
                // if a subscription price comes in as a negative then that should be caught somewhere prior to reaching the invoicing code
                if (subPriceForMonth == 0)
                {
                    break;
                }

                decimal subPriceForDay = subPriceForMonth / Utils.LastDayOfMonth(billingPeriod).Day;

                foreach(KeyValuePair<int, User> user in users)
                {
                    DateTime startDate = user.Value.ActivatedOn;
                    DateTime endDate = user.Value.DeactivatedOn;
                    DateTime currentDay = billingPeriod;
                    int activeDays = 0;

                    while (currentDay <= Utils.LastDayOfMonth(billingPeriod))
                    {
                        if (Utils.IsBetween(currentDay, startDate, endDate))
                        {
                            activeDays++;
                        }
                        currentDay = Utils.NextDay(currentDay);
                    }
                    monthlyCost += subPriceForDay * activeDays;
                }
            }

            //monthlyCost = RoundUpToHundrethsPlace(monthlyCost);
            monthlyCost = Math.Round(monthlyCost, 2, MidpointRounding.ToEven);

            runningTotal += monthlyCost;
        }

        // In case we want any decimal value after the hundreths place to mean we round up an entire cent
        private decimal RoundUpToHundrethsPlace(decimal numberToRoundUp)
        {
            decimal dollars = Math.Truncate(numberToRoundUp);
            // Round up to the nearest cent since currency is 2 decimal places
            decimal cents = numberToRoundUp - Math.Truncate(numberToRoundUp);
            // Move the decimal over 2 places then round up
            string centsString = cents.ToString();
            centsString = centsString.Substring(centsString.IndexOf('.')+1);
            int numValues = centsString.Length;
            if (numValues > 2)
            {
                cents = Convert.ToDecimal(centsString);
                // Avoiding division by 0 cases
                if (cents != 0)
                {
                    // Gives us an easier way to round up
                    decimal centsToRoundUp = cents / Convert.ToDecimal((Math.Pow(10, (numValues - 2))));
                    decimal roundedCents = Math.Ceiling(centsToRoundUp);
                    roundedCents = roundedCents / 100;
                    dollars += roundedCents;
                }
            }

            return dollars;
        }
    }
}
