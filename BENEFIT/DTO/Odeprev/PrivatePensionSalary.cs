using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Odeprev
{
    public class PrivatePensionSalary
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _salary;

        public string Salary
        {
            get { return _salary; }
            set { _salary = value; }
        }

        private string _percentage;

        public string Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }

        public PrivatePensionSalary()
        {
        }

        public PrivatePensionSalary(int id, string salary, string percentage)
        {
            this.Id = id;
            this.Salary = salary;
            this.Percentage = percentage;

        }

    }
}