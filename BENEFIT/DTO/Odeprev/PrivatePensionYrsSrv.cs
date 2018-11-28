using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Odeprev
{
    public class PrivatePensionYrsSrv
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _year;

        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private string _percentage;

        public string Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }

        public PrivatePensionYrsSrv()
        {
        }

        public PrivatePensionYrsSrv(int id, string year, string percentage)
        {
            this.Id = id;
            this.Year = year;
            this.Percentage = percentage;
        }

    }
}