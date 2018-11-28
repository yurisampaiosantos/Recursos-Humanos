using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceInternal
{
    public class UserId
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

       
    }

    
}