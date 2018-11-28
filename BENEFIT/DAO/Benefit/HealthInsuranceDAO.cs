using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Benefit;
using System.Data.OleDb;
using DATA;

namespace DAO.benefit
{
    public class HealthInsuranceDAO
    {
        public HealthInsuranceDAO()
        {
        }

        #region 

        public List<HealthInsurance> ListHealthInsurance()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, DESCRIPTION, AMOUNT FROM BE_HEALTH_INSURANCE";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<HealthInsurance> healthInsurance = new List<HealthInsurance>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            healthInsurance.Add(new HealthInsurance(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["description"]),
                                                                        Convert.ToString(dr["amount"])));

                    }

                    con.Close();

                    return healthInsurance;



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