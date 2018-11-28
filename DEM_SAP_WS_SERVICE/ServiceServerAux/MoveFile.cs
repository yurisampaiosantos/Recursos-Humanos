using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServiceServerAux
{
    class MoveFile
    {

        static MoveFile() { }

        public static void Start()
        {

            Thread thread = new Thread(new ThreadStart(doMoveFile));
            thread.Start();

        }

        public static void doMoveFile()
        {
            string folder = ConfigurationSettings.AppSettings["folder"].ToString();
            string folderdestination = ConfigurationSettings.AppSettings["folderdestination"].ToString();


            DirectoryInfo directoryPdf = new DirectoryInfo(@folder);
            DirectoryInfo diretorioDestino = new DirectoryInfo(@folderdestination);

            //Array com arquivos encontrados no diretorio definido no app.config

            FileInfo[] pdfFiles = directoryPdf.GetFiles("*.pdf");

            foreach (FileInfo pdf in pdfFiles)
            {

                System.IO.Directory.Move(folder, folderdestination);

            }


        }
           

    }
}
