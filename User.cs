using System;
using System.Collections.Generic;
using System.Text;

namespace bread2
{
    public class User
    {
        public User() { }
        public User(int id, string name, DateTime activatedOn, DateTime deactivatedOn, int customerId)
        {
            this.Id = id;
            this.Name = name;
            this.ActivatedOn = activatedOn;
            this.DeactivatedOn = deactivatedOn;
            this.CustomerId = customerId;
        }

        public int Id;
        public string Name;
        public DateTime ActivatedOn;
        public DateTime DeactivatedOn;
        public int CustomerId;
    }
}
