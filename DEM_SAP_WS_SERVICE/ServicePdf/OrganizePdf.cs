using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Encrypt;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using DATA;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;


namespace ServicePdf
{
    class OrganizePdf
    {

        static OrganizePdf() { }


        //Thread para distribuição dos arquivos PDF na pasta. 
        //Evitando travamento na execução do serviço para multiplas distribuições.

        public static void Start()
        {

            Thread thread = new Thread(new ThreadStart(doOrganizePdf));
            thread.Start();

        }
       
        public static void Stop()
        {
            
        }


        #region Método para orgnização dos arquivos do tipo pdf.

      
        public static void doOrganizePdf()
        {

         
                //try
                //{

                    //String que define o diretório e sub diretório em que será distribuido os arquivos.
                    //Definição na app.config

                    string folder = ConfigurationSettings.AppSettings["folder"].ToString();
                    string folderdestination = ConfigurationSettings.AppSettings["folderdestination"].ToString();
                              //Variáveis para criação da estrutura de pastas

                    string newDirectory = "";
                    string newSubDirectory = "";
                    string newSubDirectoryNewYear = "";
                    bool verifyExists = false;


                    DirectoryInfo directoryPdf = new DirectoryInfo(@folder);
                    DirectoryInfo diretorioDestino = new DirectoryInfo(@folderdestination);

                    //Array com arquivos encontrados no diretorio definido no app.config

                    FileInfo[] pdfFiles = directoryPdf.GetFiles("*.pdf");


                    //Rotina que distribui os arquivos nos seus respectivos diretórios.
                    //A pasta principal é criada a partir dos 5 primeiro caracteres do login do usuário
                    //ex.: os arquivos do login SAP 00000001 ficarão na pasta principal 00000...
                    //os arquivos do usuário SAP 00001000 - ficarão na pasta principal 00001
                    //Sendo assim teremos a divisão de 999 (novecentos e noventa e nove) subdiretórios por pasta principal.
                    //Os subdiretórios serão criados e nomeados a partir da matrícula SAP do usuário.
                    //O ano será o último diretório criado e seguirá o ano descrito no nome do arquivo, antes do tipo.

                    foreach (FileInfo pdf in pdfFiles)
                    {
                        if (pdf.Name.Length == 21)
                        {

                            verifyExists = false;

                            //Diretório principal (range 1-999  | 1000 - 19999 | 2000 - 2999 ...)
                            newDirectory = diretorioDestino.FullName + "\\" + pdf.Name.Substring(0, 5);

                            if (!Directory.Exists(newDirectory))
                            {
                                diretorioDestino.CreateSubdirectory(pdf.Name.Substring(0, 5));
                            }


                            //Subdiretório (login do SAP)
                            newSubDirectory = newDirectory + "\\" + pdf.Name.Substring(0, 8);

                            if (!Directory.Exists(newSubDirectory))
                            {
                                diretorioDestino.CreateSubdirectory(pdf.Name.Substring(0, 5) + "\\" + pdf.Name.Substring(0, 8));
                            }

                            //Subdiretório ano (ano de referência descrito no nome do arquivo)
                            newSubDirectoryNewYear = newSubDirectory + "\\" + pdf.Name.Substring(10, 4);

                            //Verifica se o diretório existe, caso não será criado
                            if (!Directory.Exists(newSubDirectoryNewYear))
                            {
                                diretorioDestino.CreateSubdirectory(pdf.Name.Substring(0, 5) + "\\" + pdf.Name.Substring(0, 8) + "\\" + pdf.Name.Substring(10, 4));
                            }

                            //Variáveis para configuração da criptografia do arquivo, criptografia gerada a partir da nomenclatura do arquivo até o tipo.
                            string renameFile = pdf.Name.ToString();
                            string initialPassMd5 = renameFile.Substring(0, 2);
                            string finishPassMd5 = renameFile.Substring(6, 2);
                            string messageEncrypt = pdf.Name.Substring(0, 17);

                            //Acrescenta o nome eep e a extensão do arquivo
                            renameFile = renameFile.Substring(0, 17) + "eep.pdf";

                            //Criptografa o arquivo e realiza replace nos caracteres '/' e '+', atribuindo mais segurança na decriptografia do arquivo.
                            string passEncryptMd5 = Encrypt.Crypt.EncryptString(messageEncrypt, initialPassMd5 + finishPassMd5).Replace("/", "1").Replace("+", "1");

                            if (System.IO.File.Exists(newSubDirectoryNewYear + "\\" + renameFile))
                            {

                                System.IO.File.Delete(newSubDirectoryNewYear + "\\" + renameFile);
                                verifyExists = true;
                            }


                            using (Stream input = new FileStream(pdf.FullName.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read))

                            using (Stream output = new FileStream(newSubDirectoryNewYear + "\\" + renameFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                PdfReader reader = new PdfReader(input);
                                PdfEncryptor.Encrypt(reader, output, true, passEncryptMd5, passEncryptMd5, PdfWriter.ALLOW_PRINTING);

                            }

                            //String para insert no banco dos arquivos e pastas. Este insert alimentará o sistema de visualização de demonstrativos no APEX.
                            var sqlString = "insert into eep_humanresources.dm_files (sap_number, month, year, type, path) values (:sap_number, :month, :year, :type, :path) ";

                            string pathcomplete = newSubDirectoryNewYear.Substring(2, (newSubDirectoryNewYear.Length - 2)) + "\\" + renameFile;

                            if (verifyExists == false)
                            {
                                using (OleDbConnection con = new OleDbConnection(ConnectionString.getConnection))
                                using (OleDbCommand cmd = new OleDbCommand(sqlString, con))
                                {
                                    cmd.Parameters.AddWithValue("sap_number", renameFile.Substring(0, 8));
                                    cmd.Parameters.AddWithValue("month", renameFile.Substring(8, 2));
                                    cmd.Parameters.AddWithValue("year", renameFile.Substring(10, 4));
                                    cmd.Parameters.AddWithValue("type", renameFile.Substring(14, 3).ToUpper());
                                    cmd.Parameters.AddWithValue("path", pathcomplete);


                                    using (con)
                                    {

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }

                                }
                            }



                            //Deleta o arquivo após a cópia para a pasta correta.
                            System.IO.File.Delete(pdf.FullName.ToString());

                        }

                        else
                        {
                            //Salva na tabela DM_ERROR do schema HR o nome do arquivo que esta fora de padrão
                            var sqlErrorString = "insert into eep_humanresources.dm_error (error, created_date) values (:error, :created_date) ";

                            using (OleDbConnection con = new OleDbConnection(ConnectionString.getConnection))
                            using (OleDbCommand cmd = new OleDbCommand(sqlErrorString, con))
                            {
                                cmd.Parameters.AddWithValue("error", "Nome de arquivo fora do padrão - " + pdf.Name.ToString());
                                cmd.Parameters.AddWithValue("created_date", DateTime.Now.ToString());

                                using (con)
                                {

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }

                            }
                            //Apaga o arquivo para não deixar demonstrativos decriptografados na pasta
                            System.IO.File.Delete(pdf.FullName.ToString());

                        }
                    

                    
                        

                    }


                //}

                //catch (Exception ex)
                //{
                //    throw ex;

                //}
            

        }
        #endregion



    }
}
