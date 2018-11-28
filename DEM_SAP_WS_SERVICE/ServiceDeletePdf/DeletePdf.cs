using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;

namespace ServiceDeletePdf
{
    class DeletePdf
    {

        static DeletePdf(){}

         public static void Start()
        {

            Thread thread = new Thread(new ThreadStart(doDeletePdf));
            thread.Start();
     

        }
       
        public static void Stop()
        {
        }

        public static void doDeletePdf()
        {

            try
            {
                string folder = ConfigurationSettings.AppSettings["folder"].ToString();

                DirectoryInfo directoryPdf = new DirectoryInfo(@folder);

                FileInfo[] pdfFiles = directoryPdf.GetFiles("*.pdf");


                foreach (FileInfo pdf in pdfFiles)
                {
                    if (pdf.CreationTime.AddSeconds(10) < DateTime.Now)
                    {
                        System.IO.File.Delete(pdf.FullName.ToString());

                    }


                }
            }
            catch
            {
            }
        }


    }

    
               
}
