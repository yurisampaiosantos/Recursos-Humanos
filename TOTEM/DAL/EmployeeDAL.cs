using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using DATA;
using System.Data.OleDb;




namespace DAL
{
    /// <summary>
    /// Classe responsável pelo acesso dos objetos employee ao banco de dados.
    /// </summary>
    public class EmployeeDAL
    {
        
        /// <summary>
        /// Retorna um objeto do tipo employee.
        /// Objeto modelado a partir de informações da view VW_DOMAIN_SAP_HR SCHEMA SAP_HR.
        /// Método utilizado para buscar informações quando o login for realizado pela MOD ( Mão de Obra Direta)
        /// utilizando a matricula do SAP.
        /// </summary>
        /// <param name="login">matrícula do usuário no sap</param>
        /// <returns>Objeto Employee</returns>
        public Employee getEmployeeByLogin(string login)
        {
            OleDbDataReader dr = null;
            Employee employee = new Employee();

            try
            {
                string sql = "SELECT PERNR, NOME, CPF, LOGIN, EMAIL FROM SAP_HR.VW_DOMAIN_SAP WHERE PERNR = :LOGIN";

                OleDbConnection con = new OleDbConnection(ConnectionString.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                cmd.Parameters.AddWithValue("@LOGIN", login);
 
                using (con)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                       // if (dr != null && dr.HasRows)
                      //  {

                            employee.Sap_number = Convert.ToString(dr["PERNR"]);
                            employee.Name = Convert.ToString(dr["NOME"]);
                            employee.Cpf = Convert.ToString(dr["CPF"]);
                            employee.Email = Convert.ToString(dr["EMAIL"]);
                            employee.Name = Convert.ToString(dr["NOME"]);
                            employee.Login = Convert.ToString(dr["LOGIN"]);
                      //  }

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

            return employee;

        }
       
        /// <summary>
        /// Retorna um objeto do tipo employee.
        /// Objeto modelado a partir de informações da view VW_DOMAIN_SAP_HR SCHEMA SAP_HR.
        /// Método utilizado para buscar informações quando o login for realizado pela MOI ( Mão de Obra indireta)
        /// utilizando o login do domínio eepsa.
        /// /// </summary>
        /// <param name="login">matrícula do usuário no sap</param>
        /// <returns>Objeto Employee</returns>
        public Employee getEmployeeByLoginAD(string login)
        {
            OleDbDataReader dr = null;
            Employee employee = new Employee();

            try
            {
                string sql = "SELECT PERNR, NOME, CPF, LOGIN, EMAIL FROM SAP_HR.VW_DOMAIN_SAP WHERE UPPER(LOGIN) = UPPER(:LOGIN)";

                OleDbConnection con = new OleDbConnection(ConnectionString.getConnection);
                OleDbCommand cmd = new OleDbCommand(sql, con);

                cmd.Parameters.AddWithValue("@LOGIN", login);

                using (con)
                {
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        // if (dr != null && dr.HasRows)
                        //  {

                        employee.Sap_number = Convert.ToString(dr["PERNR"]);
                        employee.Name = Convert.ToString(dr["NOME"]);
                        employee.Cpf = Convert.ToString(dr["CPF"]);
                        employee.Email = Convert.ToString(dr["EMAIL"]);
                        employee.Name = Convert.ToString(dr["NOME"]);
                        employee.Login = Convert.ToString(dr["LOGIN"]);
                        //  }

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

            return employee;

        }
    }
}
