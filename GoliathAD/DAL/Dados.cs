using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DAL
{
    public class Dados
    {
        public static string StringDeConexaoOracle
        {
            get
            {//MSDAORA                 
                //------produ��o                                                                             
                return "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=LDCRAC2-SCAN.intranet.local)(PORT= 1521)))(CONNECT_DATA=(SERVICE_NAME=CRP01.intranet.local)(SERVER=DEDICATED)));User Id=F_APP_EMRB;Password=EmRbOra�qq";
            }
        }
        public static string StringDeConexaoSqlServer
        {
            get
            {//SQL Server                 
                //------produ��o              
                return "Data Source=CDCSQLCM01;Initial Catalog=CM_ENS;User ID=f_app_pbi;Password=pbi14SQL";
            }
        }
    }
}
