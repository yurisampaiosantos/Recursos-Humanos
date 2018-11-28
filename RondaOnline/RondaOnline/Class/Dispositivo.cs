using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace RondaOnline
{
    public class Dispositivo
    {
        public static string StringDeConexao
        {
            get
            {
                return "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=LDCRAC2-SCAN.intranet.local)(PORT= 1521)))(CONNECT_DATA=(SERVICE_NAME=CRP01.intranet.local)(SERVER=DEDICATED)));User Id=F_GL_SAPRONDA;Password=enSFGLSAPRONDAora2014";
            }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public List<Dispositivo> ListaDispositivo()
        {
            string sql;
            sql = "";
            sql += "SELECT ";
            sql += " ID,IP ";
            sql += "FROM EEP_PONTO.CD_DISPOSITIVO ";
            sql += "WHERE STATUS <> 2 ";


            OleDbConnection con = new OleDbConnection(StringDeConexao);
            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            List<Dispositivo> listaDispositivo = new List<Dispositivo>();

            using (con)
            {
                con.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Dispositivo dispositivo = new Dispositivo();
                    dispositivo.Id = Convert.ToInt32(reader["ID"]);
                    dispositivo.Ip = Convert.ToString(reader["IP"]);
                    listaDispositivo.Add(dispositivo);
                }
                con.Close();
                return listaDispositivo;
            }
        }

        public void InseriorOffLine(int idDispositivo)
        {
            ///Metodo para Listar todos no banco
            ///
            string sql;
            sql = "";
            sql += "INSERT INTO  EEP_PONTO.CD_REGISTRO (DISPOSITIVO_ID,DATA) ";
            sql += "VALUES (?, SYSDATE )";
            OleDbConnection con = new OleDbConnection(StringDeConexao);
            try
            {
                OleDbCommand comando = new OleDbCommand(sql, con);
                comando.Parameters.Add("@DISPOSITIVO_ID", OleDbType.Integer).Value = idDispositivo;
                con.Open();
                comando.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }

    }
}
