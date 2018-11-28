using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading;

namespace PdfPrinter
{
    class Printer
    {

        static void Main(string[] args)
        {

           
        }
    }

    //Classe para impressão de PDF
    //Requisito: instalação do Adobe Acrobat Reader
    //Retorna true se o Adobe Acrobat Reader abrir e imprimir o arquivo. 
    //Caso ocorra algum erro será retorna false e não será tratado. Deve ser tratado na camada de visualização
    public class Pdf
    {
        public static Boolean PrintPDFs(string pdfFileName)
        {
            try
            {
                
                //Inicializa um processo do windows em modo oculto com verb print para o executável do adobe acrobat reader.
                //Aguarda 10 segundos para execução da impressão, este tempo precisa ser regulado de acordo com a porta da impressora local/ethernet/mapeamento
                Process proc = new Process();
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.Verb = "print";
                proc.StartInfo.FileName =
                @"C:\Program Files (x86)\Adobe\Reader 11.0\Reader\AcroRd32.exe";

                proc.StartInfo.UserName = "F_GL_SAP_HR_DM@eepsa.com.br";
                proc.StartInfo.Password = getPassProcess();
                proc.StartInfo.Domain = "eepsa";
                
                proc.StartInfo.Arguments = String.Format(@"/p /h {0}", pdfFileName);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;

                proc.Start();
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                
                
                //if (proc.HasExited == false)
                //{
                //    proc.WaitForExit(20000);
                //}

                for (int i = 0; i < 5; i++)
                {
                    if (!proc.HasExited)
                    {
                        proc.Refresh();
                        Thread.Sleep(2000);
                    }
                    else
                        break;
                }
                if (!proc.HasExited)
                {
                    proc.CloseMainWindow();
                }

                proc.EnableRaisingEvents = true;

                proc.Close();
                KillAdobe("AcroRd32");
                return true;

            }




            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                return false;

            }
        }

        //Geração da senha de acesso para abertura do processo.
        public static SecureString getPassProcess()
        {
            string passwordPre = "Sppe$#20@13Xx!";
            char[] passwordChars = passwordPre.ToCharArray();
            SecureString password = new SecureString();
            foreach (char c in passwordChars)
            {
                password.AppendChar(c);
            }
            return password;
        }

        //Finaliza o processo do Adobe Acrobat Reader
        private static bool KillAdobe(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(
                         clsProcess => clsProcess.ProcessName.StartsWith(name)))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }
    }
}


