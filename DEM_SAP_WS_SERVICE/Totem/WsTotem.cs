using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Totem
{
    public class WsTotem
    {


        //Método que consome o Web Service principal de abertura do demonstrativo e retorna o caminho do demonstrativo.
        public string openPdfWsSap(string matricula, string path)
        {
            WsOpenPdf.WebserviceOpenPdfSapSoapClient openPdf = new WsOpenPdf.WebserviceOpenPdfSapSoapClient();

            return openPdf.openPdf(matricula, path);
        }
        

    }
}