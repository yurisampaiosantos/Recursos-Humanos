using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATA
{
    public class StringConnection
    {

        public StringConnection()
        {
        }

        public static string getConnection
        {
            get
            {
                return "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=WCHLDEV01)(PORT= 1521)))(CONNECT_DATA=(SID=devbox)(SERVER=DEDICATED)));User Id=EEP_HUMANRESOURCES;Password=eepSA2020";
            }
        }



    }

}