using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Odeprev;
using DATA;
using System.Data.OleDb;

namespace DAO.Odeprev
{
    public class PrivatePensionSalaryDAO
    {
        public PrivatePensionSalaryDAO()
        {
        }

        #region

        public List<PrivatePensionSalary> ListPrivatePensionSalary()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, SALARY, PERCENT FROM BE_PRIVATE_PENSION_SALARY";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<PrivatePensionSalary> privatePensionSalary = new List<PrivatePensionSalary>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            privatePensionSalary.Add(new PrivatePensionSalary(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["salary"]),
                                                                        Convert.ToString(dr["percent"])));

                    }

                    con.Close();

                    return privatePensionSalary;

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