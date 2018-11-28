using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Benefit;
using System.Data.OleDb;
using DATA;

namespace DAO.benefit
{
    public class AccidentInsuranceDAO
    {
        public AccidentInsuranceDAO()
        {
        }

        #region List Accident Insurance

        public List<AccidentInsurance> ListAccidentInsurance()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, DESCRIPTION, AMOUNT FROM BE_ACCIDENT_INSURANCE";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<AccidentInsurance> accidenteInsurance = new List<AccidentInsurance>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            accidenteInsurance.Add(new AccidentInsurance(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["description"]),
                                                                        Convert.ToString(dr["amount"])));

                    }

                    con.Close();

                    return accidenteInsurance;

                   

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