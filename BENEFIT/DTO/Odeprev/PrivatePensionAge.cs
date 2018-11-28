using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Odeprev
{
    public class PrivatePensionAge
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _age;

        public string Age
        {
            get { return _age; }
            set { _age = value; }
        }

        private string _percentage;

        public string Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }

        public PrivatePensionAge()
        {
        }

        public PrivatePensionAge(int id, string age, string percentage)
        {
            this.Id = id;
            this.Age = age;
            this.Percentage = percentage;
        }
            

    }
}