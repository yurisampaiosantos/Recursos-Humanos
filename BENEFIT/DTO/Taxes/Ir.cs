using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO.Taxes
{
    public class Ir
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

        private string deduction;

        public string Deduction
        {
            get { return deduction; }
            set { deduction = value; }
        }

        public Ir()
        {
        }

        public Ir(int id, string amount_min, string amount_max, string percentage, string deduction)
        {
            this.Id = id;
            this.Amount_min = amount_min;
            this.Amount_max = amount_max;
            this.Percentage = percentage;
            this.Deduction = deduction;

        }
    }
}