using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using DATA;


namespace WebServiceInternal
{
    public class Autentication
    {
        public Autentication()
        {
        }

        public Autentication(UserId user)
        {
                                               
        }

        public bool userIsValid(UserId user)
        {
            bool result = false;

            string sql;
            sql = "";
            sql += "SELECT COUNT(1) ";
            sql += " FROM BE_USERS ";
            sql += " WHERE UPPER(FEDERAL_TAX_NUMBER) = UPPER(TRIM('" + user.Name + "')) ";
            sql += " AND UPPER(PASSWORD) = UPPER(TRIM('" + user.Password + "')) ";

            OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            using (con) 

            {
                con.Open();

                if (cmd.ExecuteScalar().ToString() == "1")
                {
                    result = true;
                }

                con.Close();

                return result;

               
            }
            
        }
   
    }

}