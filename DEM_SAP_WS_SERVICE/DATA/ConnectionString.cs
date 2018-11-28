using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DATA
{
    public class ConnectionString
    {

        public ConnectionString()
        {
        }

        //Conexão com o banco Oracle para registro das atividades realizadas pelo organizador de arquivos.

        public static string getConnection
        {
            get
            {
                //return "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=LDCDB01)(PORT= 1521)))(CONNECT_DATA=(SID=CRP)(SERVER=DEDICATED)));User Id=F_GL_HUMANRESOURCES;Password=ORAens14prod";
                return "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=LDCRAC2-SCAN)(PORT= 1521)))(CONNECT_DATA=(SERVICE_NAME=CRP01.intranet.local)(SERVER=DEDICATED)));User Id=F_GL_HUMANRESOURCES;Password=ORAens14prod";
             
            }

        }



    }
}
