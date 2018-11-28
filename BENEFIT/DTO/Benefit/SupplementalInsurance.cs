using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Benefit
{
    public class SupplementalInsurance
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _amount;

        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public SupplementalInsurance()
        {
        }

        public SupplementalInsurance(int id, string description, string amount)
        {
            this.Id = id;
            this.Description = description;
            this.Amount = amount;
        }

        private string _displayDescription;

        public string DisplayDescription
        {
            get { return Description + " - " + Amount; }
            set { _displayDescription = value; }
        }
        

    }
}