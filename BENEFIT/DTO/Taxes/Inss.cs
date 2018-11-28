using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Taxes
{
    public class Inss
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _amount_min;

        public string Amount_min
        {
            get { return _amount_min; }
            set { _amount_min = value; }
        }

        private string _amount_max;

        public string Amount_max
        {
            get { return _amount_max; }
            set { _amount_max = value; }
        }

        private string _percentage;

        public string Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }

        public Inss()
        {
        }

        public Inss(int id, string amount_min, string amount_max, string percentage)
        {
            this.Id = id;
            this.Amount_min = _amount_min;
            this.Amount_max = amount_max;
            this.Percentage = percentage; 

        }


    }
}