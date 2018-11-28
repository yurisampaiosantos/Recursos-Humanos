using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Taxes;
using DATA;
using System.Data.OleDb;


namespace DAO.Taxes
{
    public class IrDAO
    {
        public IrDAO()
        {
        }

        #region
       
        public List<Ir> ListIr()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, AMOUNT_MIN, AMOUNT_MAX, PERCENT, DEDUCTION FROM BE_IR";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<Ir> ir = new List<Ir>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            ir.Add(new Ir(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["amount_min"]),
                                                                        Convert.ToString(dr["amount_max"]),
                                                                        Convert.ToString(dr["percent"]),
                                                                        Convert.ToString(dr["deduction"])));

                    }

                    con.Close();

                    return ir;

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