using System;
using System.Collections.Generic;
using System.Text;
using Modelo;
using System.Data.OleDb;
using System.Data;

namespace DAL
{
    public class PBI_OracleDAL
    {
        public void Inserir()
        {
            ///Metodo para incluir no banco
            ///
            PBI_SqlServerDAL pBI_SqlServerDAL = new PBI_SqlServerDAL();
            List<PBI_SqlServer> listPBI_SqlServer = new List<PBI_SqlServer>();
            listPBI_SqlServer = pBI_SqlServerDAL.Listagem();

            Excluir();
            using (OleDbConnection con = new OleDbConnection(Dados.StringDeConexaoOracle))
            {
                con.Open();

                OleDbCommand comando = con.CreateCommand();
                OleDbTransaction transaction;
                transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
                // Assign transaction object for a pending local transaction
                comando.Transaction = transaction;
                string sql = "";
                try
                {

                    foreach (PBI_SqlServer pBI_SqlServer in listPBI_SqlServer) // Loop through List with foreach
                    {
                        sql = "";
                        sql += "Insert into GOLIATH.USUARIO_CARGA_AD ";
                        sql += "(NOME,  LOGIN,  OU,  CPF,  CC,  MATRICULA,  EMAIL,  EMPRESA,  DEPARTAMENTO,  NEGOCIO,  ESCRITORIO,  LIDER) ";
                        sql += "Values ";
                        sql += "  (?,?,?,?,?,?,?,?,?,?,?,?) ";

                        comando.CommandText = sql;
                        comando.Parameters.Clear();

                        comando.Parameters.Add("0", OleDbType.VarChar).Value = pBI_SqlServer.Nome_Completo;
                        comando.Parameters.Add("1", OleDbType.VarChar).Value = pBI_SqlServer.Login;
                        comando.Parameters.Add("2", OleDbType.VarChar).Value = pBI_SqlServer.Ou;
                        comando.Parameters.Add("3", OleDbType.VarChar).Value = pBI_SqlServer.Cpf;
                        comando.Parameters.Add("4", OleDbType.VarChar).Value = pBI_SqlServer.Cc;
                        comando.Parameters.Add("5", OleDbType.VarChar).Value = pBI_SqlServer.Matricula;
                        comando.Parameters.Add("6", OleDbType.VarChar).Value = pBI_SqlServer.EMail;
                        comando.Parameters.Add("7", OleDbType.VarChar).Value = pBI_SqlServer.Empresa;
                        comando.Parameters.Add("8", OleDbType.VarChar).Value = pBI_SqlServer.Departamento;
                        comando.Parameters.Add("9", OleDbType.VarChar).Value = pBI_SqlServer.Descricao;                        
                        comando.Parameters.Add("10", OleDbType.VarChar).Value = pBI_SqlServer.Escritorio;
                        comando.Parameters.Add("11", OleDbType.VarChar).Value = pBI_SqlServer.Gerente;

                        comando.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    con.Close();
                }
                Atualizar();
            }
        }
      
        public void Excluir()
        {
            ///Metodo para Excluir o registro no banco
            ///
            OleDbConnection con = new OleDbConnection(Dados.StringDeConexaoOracle);
            string sql = "";
            sql += "Delete GOLIATH.USUARIO_CARGA_AD";
            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        public void Atualizar()
        {
            ///Metodo para Excluir o registro no banco
            ///
            OleDbConnection con = new OleDbConnection(Dados.StringDeConexaoOracle);
            string sql = "";
            sql += "GOLIATH.PRC_ATUALIZAR_AD";
            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }
}
