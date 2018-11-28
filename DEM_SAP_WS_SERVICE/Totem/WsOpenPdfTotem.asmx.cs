using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PdfPrinter;
using System.Configuration;

namespace Totem
{
    /// <summary>
    /// Web Service que consumirá o WS principal de demonstrativo e mandará uma impressão para impressora local do totem.
    /// </summary>
    [WebService(Namespace = "http://eepsa.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    [System.Web.Script.Services.ScriptService]
    public class WsOpenPdfTotem : System.Web.Services.WebService
    {
        //Recebe a matricula do SAP do usuário e o caminho do arquivo e retorna true se o demonstrativo for impresso no Totem
        [WebMethod]
        public bool OpenPdfMod(string matricula, string path)
        {
     
            bool isValidPrint = false;
                        
            WsOpenPdf.WebserviceOpenPdfSapSoapClient Pdf = new WsOpenPdf.WebserviceOpenPdfSapSoapClient();

            string pathPrinterWeb = Pdf.openPdf(matricula, path) ;
           
            string pathPrinter = ConfigurationManager.AppSettings["pathPrinter"].ToString() + pathPrinterWeb.Substring(32, (pathPrinterWeb.Length-32));

            if (PdfPrinter.Pdf.PrintPDFs(pathPrinter))
        
            {
                isValidPrint = true;
            }
            return isValidPrint;
        }
    }
}
