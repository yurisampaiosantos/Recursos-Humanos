using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Benefit;
using System.Data.OleDb;
using DATA;

namespace DAO.benefit
{
    public class MealVoucherDAO
    {
        public MealVoucherDAO()
        {
        }

        #region

        public List<MealVoucher> ListMealVoucher()
        {
            OleDbDataReader dr = null;

            try
            {
                string sql = "SELECT ID, DESCRIPTION, AMOUNT FROM BE_MEAL_VOUCHER";

                OleDbConnection con = new OleDbConnection(StringConnection.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                using (con)
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    List<MealVoucher> mealVoucher = new List<MealVoucher>();

                    while (dr.Read())
                    {
                        if (dr != null && dr.HasRows)
                            mealVoucher.Add(new MealVoucher(Convert.ToInt32(dr["id"]),
                                                                        Convert.ToString(dr["description"]),
                                                                        Convert.ToString(dr["amount"])));

                    }

                    con.Close();

                    return mealVoucher;



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