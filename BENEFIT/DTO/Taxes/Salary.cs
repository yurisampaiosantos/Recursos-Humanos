using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Taxes
{
    public class Salary
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _amount;

        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public Salary()
        {
        }

        public Salary(int id, string amount)
        {
            this.Id = id;
            this.Amount = amount;
        }
    }
}