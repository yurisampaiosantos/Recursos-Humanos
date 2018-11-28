using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Threading;

namespace WebserviceOpenPdf
{
    /// <summary>
    /// Web Service que recebe a matrícula SAP do usuário e o caminho. Após decriptografar o arquivo retorna o caminho físico para abertura do mesmo pelo browser.
    /// </summary>
    [WebService(Namespace = "http://eepsa.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    [System.Web.Script.Services.ScriptService]
    public class WebserviceOpenPdfSap : System.Web.Services.WebService
    {

        [WebMethod]
        public string openPdf(string matricula, string path)
        {

            try
            {

                //String de definição da pasta onde ficará os arquivos descriptografados para abertura no browser.
                //Esse diretórtio será esvaziado de acordo com o intervalo definido no serviço de deleção.
                string folder = ConfigurationManager.AppSettings["folder"].ToString();


                DirectoryInfo directoryPdf = new DirectoryInfo(@folder);
                path = "\\\\" + path;
                
                //Variáveis de configuração para decriptografia do arquivo
                string pathPdfFile = path;

                string initialPassMd5 = matricula.Substring(0, 2);
                string finishPassMd5 = matricula.Substring(6, 2);
                string decryptNameFile = matricula.Substring(0, 17);

                //decriptografia realizada com fórmula de replace utilizada na criptografia.
                string passEncryptMd5 = Encrypt.Crypt.EncryptString(decryptNameFile, initialPassMd5 + finishPassMd5).Replace("/","1").Replace("+","1");
                
                using (Stream input = new FileStream(pathPdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream output = new FileStream(folder + "\\" + passEncryptMd5 + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None))
                    
               
                {
                    PdfReader reader = new PdfReader(input, new System.Text.ASCIIEncoding().GetBytes(passEncryptMd5));

                    PdfEncryptor.Encrypt(reader, output, false, "", "", PdfWriter.ALLOW_PRINTING);
                 
                    
                }

                //Necessário definir o servidor IIS que ficará o diretório lógico para abertura dos demonstrativos.
                //Pode ser refatorado para utilização do caminho a partir do app.config.
                return "http://wdciis03.intranet.local/demonstrativos/" + passEncryptMd5 + ".pdf";
               

            }
            catch
            {
                throw new Exception();

            }
        

        }

        [WebMethod]
        public string printFileTotem(string matricula, string path)
        {
            //String de definição da pasta onde ficará os arquivos descriptografados para abertura no browser.
            //Esse diretórtio será esvaziado de acordo com o intervalo definido no serviço de deleção.
            string folder = ConfigurationManager.AppSettings["folder"].ToString();


            DirectoryInfo directoryPdf = new DirectoryInfo(@folder);
            path = "\\\\" + path;

            //Variáveis de configuração para decriptografia do arquivo
            string pathPdfFile = path;

            string initialPassMd5 = matricula.Substring(0, 2);
            string finishPassMd5 = matricula.Substring(6, 2);
            string decryptNameFile = matricula.Substring(0, 17);

            //decriptografia realizada com fórmula de replace utilizada na criptografia.
            string passEncryptMd5 = Encrypt.Crypt.EncryptString(decryptNameFile, initialPassMd5 + finishPassMd5).Replace("/", "1").Replace("+", "1");

            using (Stream input = new FileStream(pathPdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream output = new FileStream(folder + "\\" + passEncryptMd5 + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfReader reader = new PdfReader(input, new System.Text.ASCIIEncoding().GetBytes(passEncryptMd5));

                PdfEncryptor.Encrypt(reader, output, false, "", "", PdfWriter.ALLOW_PRINTING);


            }

            //Necessário definir o servidor IIS que ficará o diretório lógico para abertura dos demonstrativos.
            //Pode ser refatorado para utilização do caminho a partir do app.config.
            return folder + "\\" + passEncryptMd5 + ".pdf";

        }
             
    }
}
