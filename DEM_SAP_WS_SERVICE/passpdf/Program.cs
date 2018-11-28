using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encrypt;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using DATA;

namespace passpdf
{
    class Program
    {

        

        static void Main(string[] args)
        {

            try
            {
                string folder = ConfigurationSettings.AppSettings["folder"].ToString();
                string folderdstination = ConfigurationSettings.AppSettings["folderdstination"].ToString();
                


                string newDirectory = "";
                string newSubDirectory = "";
                string newSubDirectoryNewYear = "";


                DirectoryInfo directoryPdf = new DirectoryInfo(@folder);
                DirectoryInfo diretorioDestino = new DirectoryInfo(@folderdstination);

                FileInfo[] pdfFiles = directoryPdf.GetFiles("*.pdf");





                foreach (FileInfo pdf in pdfFiles)
                {

                    //Main directory - range 1-999  | 1000 - 19999 | 2000 - 2999

                    newDirectory = diretorioDestino.FullName + "\\" + pdf.Name.Substring(0, 5);

                    if (!Directory.Exists(newDirectory))
                    {
                        diretorioDestino.CreateSubdirectory(pdf.Name.Substring(0, 5));
                    }


                    //Subdirectory login sap user

                    newSubDirectory = newDirectory + "\\" + pdf.Name.Substring(0, 8);

                    if (!Directory.Exists(newSubDirectory))
                    {
                        diretorioDestino.CreateSubdirectory(pdf.Name.Substring(0, 5) + "\\" + pdf.Name.Substring(0, 8));
                    }

                    //Subdirectory year

                    newSubDirectoryNewYear = newSubDirectory + "\\" + pdf.Name.Substring(10, 4);

                    if (!Directory.Exists(newSubDirectoryNewYear))
                    {
                        diretorioDestino.CreateSubdirectory(pdf.Name.Substring(0, 5) + "\\" + pdf.Name.Substring(0, 8) + "\\" + pdf.Name.Substring(10, 4));
                    }


                    string renameFile = pdf.Name.ToString();
                    string initialPassMd5 = renameFile.Substring(0, 2);
                    string finishPassMd5 = renameFile.Substring(6, 2);
                    string messageEncrypt = pdf.Name.Substring(0, 17);
                    renameFile = renameFile.Substring(0, 17) + "MD5.pdf";

                    string passEncryptMd5 = Encrypt.Crypt.EncryptString(messageEncrypt, initialPassMd5 + finishPassMd5).Replace("/","1").Replace("+","1");
                    

                    

                    using (Stream input = new FileStream(pdf.FullName.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read))
                    
                    using (Stream output = new FileStream(newSubDirectoryNewYear + "\\" + renameFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        PdfReader reader = new PdfReader(input);
                        PdfEncryptor.Encrypt(reader, output, true, passEncryptMd5, passEncryptMd5, PdfWriter.ALLOW_PRINTING);

                    }



                    var stringSql = "insert into dm_files (sap_number, month, year, type, path) values (:sap_number, :month, :year, :type, :path) ";
                    
                    string pathcomplete = newSubDirectoryNewYear.Substring(2, (newSubDirectoryNewYear.Length - 2)) + "\\" + renameFile;


                    using(OleDbConnection con = new OleDbConnection(ConnectionString.getConnection))
                    using (OleDbCommand cmd = new OleDbCommand(stringSql, con))
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
                        // System.Diagnostics.Process.Start(@"C:\Temp\pdf\"+renameFile+".pdf");
                    }
                    System.IO.File.Delete(pdf.FullName.ToString());

                }

            }
            catch (Exception ex)
            {

            }
            }

        
        
        }
    }

