using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Odeprev;
using DATA;
using System.Data.OleDb;

namespace DAO.Odeprev
{
    public class PrivatePensionAgeDAO
    {

        public PrivatePensionAgeDAO()
        {
        }

        #region

        public List<PrivatePensionAge> ListPrivatePensionAge()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, AGE, PERCENT FROM BE_PRIVATE_PENSION_AGE";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<PrivatePensionAge> privatePensionAge = new List<PrivatePensionAge>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            privatePensionAge.Add(new PrivatePensionAge(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["age"]),
                                                                        Convert.ToString(dr["percent"])));

                    }

                    con.Close();

                    return privatePensionAge;

                }

            }

            catch (OleDbException ex)
            {
                throw ex;
            }

            finally
            {
                dr.Close();

            }

        }


        #endregion

    }
}