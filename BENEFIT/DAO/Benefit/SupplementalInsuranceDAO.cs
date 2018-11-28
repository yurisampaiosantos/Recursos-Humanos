using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Benefit;
using System.Data.OleDb;
using DATA;

namespace DAO.benefit
{
    public class SupplementalInsuranceDAO
    {
        public SupplementalInsuranceDAO()
        {
        }


        #region

        public List<SupplementalInsurance> ListSupplementalInsurance()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, DESCRIPTION, AMOUNT FROM BE_SUPPLEMENTAL_INSURANCE";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<SupplementalInsurance> supplementalInsurance = new List<SupplementalInsurance>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            supplementalInsurance.Add(new SupplementalInsurance(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["description"]),
                                                                        Convert.ToString(dr["amount"])));

                    }

                    con.Close();

                    return supplementalInsurance;

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