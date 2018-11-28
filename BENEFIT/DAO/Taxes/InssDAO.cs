using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Taxes;
using DATA;
using System.Data.OleDb;


namespace DAO.Taxes
{
    public class InssDAO
    {
        public InssDAO()
        {
        }

        #region

        public List<Inss> ListInss()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, AMOUNT_MIN, AMOUNT_MAX, PERCENT FROM BE_INSS";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<Inss> inss = new List<Inss>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            inss.Add(new Inss(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["amount_min"]),
                                                                        Convert.ToString(dr["amount_max"]),
                                                                        Convert.ToString(dr["percent"])));

                    }

                    con.Close();

                    return inss;

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