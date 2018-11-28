using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Taxes;
using System.Data.OleDb;
using DATA;


namespace DAO.Taxes
{
    public class SalaryDAO
    {
        public SalaryDAO()
        {
        }

        #region

        public List<Salary> ListSalary()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, AMOUNT FROM BE_SALARY";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<Salary> salary = new List<Salary>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            salary.Add(new Salary(Convert.ToInt32(dr["id"]),
                                                  Convert.ToString(dr["amount"])));

                    }

                    con.Close();

                    return salary;

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