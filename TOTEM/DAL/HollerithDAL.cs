using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DATA;
using Entities;
using System.Data.OleDb;

namespace DAL
{
    /// <summary>
    /// Classe responsável pelo acesso dos objetos de hollerith ao banco de dados
    /// </summary>
    public class HollerithDAL
    {
        /// <summary>
        /// Retorna os últimos seis demonstrativos do tipo contra-cheques, plr e décimo terceiro. 
        /// </summary>
        /// <param name="cod_sap">Código SAP do integrante</param>
        /// <returns>lista de objetos do tipo Hollerith de demosntrativos</returns>
        public List<Hollerith> getLastSixHollerithDEM(string cod_sap)
        {
            OleDbDataReader dr = null;
            List<Hollerith> hollerith = new List<Hollerith>();

            try
            {
                string sql = "SELECT * FROM ( SELECT ID, SAP_NUMBER, MONTH, YEAR, TYPE, PATH FROM EEP_HUMANRESOURCES.DM_FILES WHERE SAP_NUMBER = :COD_SAP AND (TYPE = 'DEM' OR TYPE = 'DEC' OR TYPE = 'PLR') ORDER BY MONTH DESC) WHERE ROWNUM < 7 ORDER BY MONTH ASC";

                OleDbConnection con = new OleDbConnection(ConnectionString.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("@COD_SAP", cod_sap);

                using (con)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        if (dr != null && dr.HasRows)
                        {
                            hollerith.Add(new Hollerith(Convert.ToInt32(dr["ID"]),
                                Convert.ToString(dr["SAP_NUMBER"]),
                                Convert.ToString(dr["MONTH"]),
                                Convert.ToString(dr["YEAR"]),
                                Convert.ToString(dr["TYPE"]),
                                Convert.ToString(dr["PATH"])));
                        }

                    }

                    con.Close();
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
            return hollerith;


        }
        /// <summary>
        ///  Retorna os últimos seis demonstrativos de férias. 
        /// </summary>
        /// <param name="cod_sap">Código SAP do integrante</param>
        /// <returns>lista de objetos do tipo Hollerith de férias</returns>
        public List<HollerithFer> getLastSixHollerithFER(string cod_sap)
        {
            OleDbDataReader dr = null;
            List<HollerithFer> hollerithFer = new List<HollerithFer>();

            try
            {
                string sql = "SELECT * FROM ( SELECT ID, SAP_NUMBER, MONTH, YEAR, TYPE, PATH FROM EEP_HUMANRESOURCES.DM_FILES WHERE SAP_NUMBER = :COD_SAP AND TYPE = 'FER' ORDER BY MONTH DESC) WHERE ROWNUM < 7 ORDER BY MONTH ASC";

                OleDbConnection con = new OleDbConnection(ConnectionString.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("@COD_SAP", cod_sap);

                using (con)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        if (dr != null && dr.HasRows)
                        {
                            hollerithFer.Add(new HollerithFer(Convert.ToInt32(dr["ID"]),
                                Convert.ToString(dr["SAP_NUMBER"]),
                                Convert.ToString(dr["MONTH"]),
                                Convert.ToString(dr["YEAR"]),
                                Convert.ToString(dr["TYPE"]),
                                Convert.ToString(dr["PATH"])));
                        }

                    }

                    con.Close();
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
            return hollerithFer;

        }
        /// <summary>
        /// Retorna os últimos seis demonstrativos de informe de rendimento. 
        /// </summary>
        /// <param name="cod_sap">Código SAP do integrante</param>
        /// <returns>lista de objetos do tipo Hollerith de informe de rendimento</returns>
        public List<HollerithInf> getLastSixHollerithINF(string cod_sap)
        {
            OleDbDataReader dr = null;
            List<HollerithInf> hollerithInf = new List<HollerithInf>();

            try
            {
                string sql = "SELECT * FROM ( SELECT ID, SAP_NUMBER, MONTH, YEAR, TYPE, PATH FROM EEP_HUMANRESOURCES.DM_FILES WHERE SAP_NUMBER = :COD_SAP AND TYPE = 'INF' ORDER BY MONTH DESC) WHERE ROWNUM < 7 ORDER BY MONTH ASC";

                OleDbConnection con = new OleDbConnection(ConnectionString.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("@COD_SAP", cod_sap);

                using (con)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        if (dr != null && dr.HasRows)
                        {
                            hollerithInf.Add(new HollerithInf(Convert.ToInt32(dr["ID"]),
                                Convert.ToString(dr["SAP_NUMBER"]),
                                Convert.ToString(dr["MONTH"]),
                                Convert.ToString(dr["YEAR"]),
                                Convert.ToString(dr["TYPE"]),
                                Convert.ToString(dr["PATH"])));
                        }

                    }
                    con.Close();
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
            return hollerithInf;

        }
        /// <summary>
        /// Retorna todos os demonstrativos de todos os tipos
        /// Método não utilizado no início do projeto (06/01/2014)
        /// </summary>
        /// <param name="cod_sap">Campo do tipo VARCHAR2 na base Oracle</param>
        /// <returns>Lista de objetos do tipo hollerith</returns>
        public List<Hollerith> getAllHollerith(string cod_sap)
        {
            OleDbDataReader dr = null;
            List<Hollerith> hollerithALL = new List<Hollerith>();

            try
            {
                string sql = "SELECT ID, SAP_NUMBER, MONTH, YEAR, TYPE, PATH FROM EEP_HUMANRESOURCES.DM_FILES WHERE SAP_NUMBER = @cod_sap ORDER BY MONTH";

                OleDbConnection con = new OleDbConnection(ConnectionString.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("@cod_sap", cod_sap);

                using (con)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        if (dr != null && dr.HasRows)
                        {
                            hollerithALL.Add(new Hollerith(Convert.ToInt32(dr["ID"]),
                                Convert.ToString(dr["SAP_NUMBER"]),
                                Convert.ToString(dr["MONTH"]),
                                Convert.ToString(dr["YEAR"]),
                                Convert.ToString(dr["TYPE"]),
                                Convert.ToString(dr["PATH"])));
                        }

                        con.Close();
                        
                    }
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

            return hollerithALL;

        }
    }
}
