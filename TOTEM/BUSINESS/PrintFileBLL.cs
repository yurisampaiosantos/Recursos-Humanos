using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading;
using PdfPrintingNet;
using System.ComponentModel;
using System.Security.Principal;
using System.Configuration;
using System.DirectoryServices;
using System.IO;



namespace BUSINESS
{
    /// <summary>
    /// Construtor padrão.
    /// </summary>
    public class PrintFileBLL
    {

    }

    /// <summary>
    /// Classe responsável pela rotina de impressão
    /// Utiliza a biblioteca de impressão pdfprintingNet
    /// </summary>
    public class PrintFile
    {
        bool result = false;

        /// <summary>
        /// Faz a impressão do arquivo a partir do caminho que é passado.
        /// </summary>
        /// <param name="cod_sap_print">Matricula do integrante no SAP</param>
        /// <param name="path">Caminho do arquivo para impressão</param>
        /// <param name="pass">senha do arquivo</param>
        /// <returns>true se impresso e false caso ocorra erro.</returns>
        public bool printFileLocalWithPath(string cod_sap_print, string path, string pass)
        {
            HollerithBLL hollerithBll = new HollerithBLL();
            var pdfPrint = new PdfPrint("Estaleiro Enseada do Paragua?u S.A.", "58505c573326562b5d505b2a44515c");
            string caminhoImpressao = hollerithBll.getFileServerToLocal(hollerithBll.getPathFilePrint(cod_sap_print, path));

            var print = pdfPrint.Print(caminhoImpressao, "");

            if (print == PdfPrint.Status.OK)
            {
                result = true;
                FileInfo fileDelete = new FileInfo(caminhoImpressao);
                fileDelete.Delete();
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Codigos de erro da biblioteca de impressão.
        /// Não utilizado no início do projeto.
        /// </summary>
        /// <param name="status">Status de impressão do objeto PdfPrint</param>
        /// <returns>String com status</returns>
        private string DecodeStatusCode(PdfPrint.Status status)
        {
            switch (status)
            {
                case PdfPrint.Status.OK:
                    return "OK";
                case PdfPrint.Status.FILE_DOESNT_EXIST:
                    return "Filename doesn't exist";
                case PdfPrint.Status.CANNOT_PRINT_FILE:
                    return "Cannot print file";
                case PdfPrint.Status.PRINTER_DOESNT_EXIST:
                    return "Printer doesn't exist";
                case PdfPrint.Status.INVALID_DEVMOD:
                    return "Invalid printer properties structure.";
                case PdfPrint.Status.NOT_AVAILABLE_PRINTER_PROPERTIES:
                    return "Not available printer properties";
                case PdfPrint.Status.CANT_INITIALIZE_PRINTER:
                    return "Can't initialize printer";
                case PdfPrint.Status.PASSWORD_INVALID:
                    return "Invalid password";
                case PdfPrint.Status.INVALID_PDF:
                    return "Invalid pdf";
                case PdfPrint.Status.FILENAME_NOT_SET:
                    return "File name not set";
                case PdfPrint.Status.PASSWORD_NOT_PROVIDED:
                    return "PDF is password protected and password isn't provided.";
                case PdfPrint.Status.UNKNOWN_ERROR:
                    return "Unknown error";
                case PdfPrint.Status.INVALID_PRINT_RANGE:
                    return "Invalid print range";
                case PdfPrint.Status.PAGE_NUMBER_DOESNT_EXIST:
                    return "Page number doesn't exist";
            }
            return "Unknown error.";
        }
    }


}



