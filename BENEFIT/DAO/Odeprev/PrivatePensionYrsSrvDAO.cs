using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Odeprev;
using DATA;
using System.Data.OleDb;

namespace DAO.Odeprev
{
    public class PrivatePensionYrsSrvDAO
    {
        public PrivatePensionYrsSrvDAO()
        {
        }

        #region
        
        public List<PrivatePensionYrsSrv> ListPrivatePensionYrsSrv()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, YEAR, PERCENT FROM BE_PRIVATE_PENSION_YRS_SRV";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<PrivatePensionYrsSrv> privatePensionYrsSrv = new List<PrivatePensionYrsSrv>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            privatePensionYrsSrv.Add(new PrivatePensionYrsSrv(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["year"]),
                                                                        Convert.ToString(dr["percent"])));

                    }

                    con.Close();

                    return privatePensionYrsSrv;

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