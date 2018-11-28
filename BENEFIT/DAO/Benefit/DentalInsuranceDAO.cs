using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Benefit;
using System.Data.OleDb;
using DATA;

namespace DAO.benefit
{
    public class DentalInsuranceDAO
    {

        public DentalInsuranceDAO()
        {
        }

        #region List Dental Insurance

        public List<DentalInsurance> ListDentalInsurance()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, DESCRIPTION, AMOUNT FROM BE_DENTAL_INSURANCE";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<DentalInsurance> dentalInsurance = new List<DentalInsurance>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            dentalInsurance.Add(new DentalInsurance(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["description"]),
                                                                        Convert.ToString(dr["amount"])));

                    }

                    con.Close();

                    return dentalInsurance;



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