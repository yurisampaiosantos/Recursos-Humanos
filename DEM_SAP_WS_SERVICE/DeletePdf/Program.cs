using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace DeletePdf
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //String que indica a pasta em que o serviço irá deletar os arquivos. Endereço no arquivo app.config.

            string folder = ConfigurationSettings.AppSettings["folder"].ToString();
            DirectoryInfo directoryPdf = new DirectoryInfo(@folder);

            //Array que guarda nome de todos os arquivos PDF´s que se encontra no caminho informado no app.config.

            FileInfo[] pdfFiles = directoryPdf.GetFiles("*.pdf");

            //Verifica tempo de criação dos arquivos encontrados, se for maior do que 5 segundos deleta o arquivo.
            
            foreach (FileInfo pdf in pdfFiles)
            {
                if (pdf.CreationTime.AddSeconds(5) < DateTime.Now)
                {
                    System.IO.File.Delete(pdf.FullName.ToString());

                }


            }

        }
    }
}
