using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
//using System.Data.Services.Client;
using System.Configuration;

/// <summary>
/// Classe com os campos que serão enviados para o orchestrator
/// </summary>

namespace ConnectAD
{
    /// <summary>
    /// 
    /// </summary>
    public class AD
    {
        /// <summary>
        /// Metodo de envio para o Orchestrator
        /// </summary>
        /// <param name="PERNR">MATRICULA</param>
        /// <param name="MASSN">TIPO DE MEDIDA</param>
        /// <param name="BEGDA">VALIDO</param>
        /// <param name="CPF_NR">CPF</param>
        /// <param name="CNAME">NOME COMPLETO</param>
        /// <param name="BUKRS">EMPRESA - CODIGO</param>
        /// <param name="BUTXT">EMPRESA - DESCRIÇÃO</param>        
        /// <param name="ORGEH">UNIDADE ORGANIZACIONAL - CODIGO</param>
        /// <param name="ORGTX">UNIDADE ORGANIZACIONAL - DESCRIÇÃO</param>
        /// <param name="KOSTL">CENTRO DE CUSTO</param>
        /// <param name="BTRTL">SUBÁREA - CODIGO</param>
        /// <param name="BTEXT">SUBÁREA - DESCRIÇÃO</param>
        /// <param name="KONSL">LOCALIDADE - CODIGO</param>   
        /// <param name="KONSLTX">LOCALIDADE - DESCRIÇÃO</param>   
        /// <param name="RH_GET_MANAGER_ASSIGNMENT">LIDER</param>
        /// <param name="STELL">CARGO - CODIGO</param>
        /// <param name="STEXT">CARGO - DESCRIÇÃO</param>
        /// <param name="PERSK">SUBGRUPO - CODIGO</param>
        /// <param name="PTEXT">SUBGRUPO - DESCRIÇÃO</param>
        /// <param name="GBDAT">DATA DE NASCIMENTO</param>
        /// <param name="MASSG">MOTIVO DA DEMISSÃO</param>
        /// <param name="LOGIN_SUGERIDO">LOGIN SUGERIDO</param>
        /// <param name="NIVEL_BAS">NIVEL BASICO</param>
        /// <param name="NIVEL_INT">NIVEL INTERMEDIARIO</param>
        /// <param name="NIVEL_AVA">NIVEL AVANCADO</param>
        /// <param name="LTEXT">TEXTO CENTRO DE CUSTO</param>
        /// <param name="NAME_TEXT">USUAROI MODIFICADOR </param>
        
     
        public void Send(string PERNR, string MASSN, string BEGDA, string CPF_NR, string CNAME, string BUKRS, string BUTXT, string ORGEH, string ORGTX, string KOSTL, string KONSLTX,
                         string BTRTL, string BTEXT, string KONSL, string RH_GET_MANAGER_ASSIGNMENT, string STELL, string STEXT, string PERSK, string PTEXT, string GBDAT, string MASSG, string LOGIN_SUGERIDO,
                         string NIVEL_BAS, string NIVEL_INT, string NIVEL_AVA, string LTEXT, string NAME_TEXT)
        {
            //Captura no WEB.config o GUID 
            string guid = ConfigurationManager.AppSettings["Guid"];
            //Cria o Runbook do Orchestrator      
            Guid runbookId = new Guid(guid);
            StringBuilder parametersXml = new StringBuilder();
            Hashtable parameterValues = new Hashtable();
            //Campos que recebem as informações do PI
            parameterValues.Add("PERNR", PERNR);
            parameterValues.Add("MASSN", MASSN);
            parameterValues.Add("BEGDA", BEGDA);
            parameterValues.Add("CPF_NR", CPF_NR);
            parameterValues.Add("CNAME", CNAME);
            parameterValues.Add("BUKRS", BUKRS);
            parameterValues.Add("BUTXT", BUTXT);
            parameterValues.Add("ORGEH", ORGEH);
            parameterValues.Add("ORGTX", ORGTX);
            parameterValues.Add("KOSTL", KOSTL);
            parameterValues.Add("BTRTL", BTRTL);
            parameterValues.Add("BTEXT", BTEXT);
            parameterValues.Add("KONSL", KONSL);
            parameterValues.Add("KONSLTX", KONSLTX);
            parameterValues.Add("RH_GET_MANAGER_ASSIGNMENT", RH_GET_MANAGER_ASSIGNMENT);
            parameterValues.Add("STELL", STELL);
            parameterValues.Add("STEXT", STEXT);
            parameterValues.Add("PERSK", PERSK);
            parameterValues.Add("PTEXT", PTEXT);
            parameterValues.Add("GBDAT", GBDAT);
            parameterValues.Add("LOGIN_SUGERIDO", LOGIN_SUGERIDO);
            parameterValues.Add("NIVEL_BAS", NIVEL_BAS);
            parameterValues.Add("NIVEL_INT", NIVEL_INT);
            parameterValues.Add("NIVEL_AVA", NIVEL_AVA);
            parameterValues.Add("LTEXT", LTEXT);
            parameterValues.Add("NAME_TEXT", NAME_TEXT);            
            //Captura do Web.config o Link do Orchestrator            
            string serviceRoot = ConfigurationManager.AppSettings["Webservice"];

            SCOService.OrchestratorContext context = new SCOService.OrchestratorContext(new Uri(serviceRoot));
            //Passando as credencias de acesso do IIS do Orchestrator
            //context.Credentials = new System.Net.NetworkCredential("ORC_SVC", "1234abc@", "eepsa");
            context.Credentials = new System.Net.NetworkCredential("F_APP_ORC_SVC", "1234abc@", "INTRANET");

            var runbookParams = context.RunbookParameters.Where(runbookParam => runbookParam.RunbookId == runbookId && runbookParam.Direction == "In");
            try
            {
                if (runbookParams != null && runbookParams.Count() > 0)
                {
                    //Gerando o XML a ser enviado pro Orchestrator
                    parametersXml.Append("<Data>");
                    foreach (var param in runbookParams)
                    {
                        //Os preenchendo os parametros com os valores do PI
                        parametersXml.AppendFormat("<Parameter><ID>{0}</ID><Value>{1}</Value></Parameter>", param.Id.ToString("B"), parameterValues[param.Name]);
                    }
                    parametersXml.Append("</Data>");
                }
            }
            catch (Exception ex)
            {
                string strPathFile = @"C:\temp\erro.txt";
                try
                {
                    using (FileStream fs = File.Create(strPathFile))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            string erro = "";
                            foreach (var param in runbookParams)
                            {
                                erro += parameterValues[param.Name];
                            }

                            sw.WriteLine(PERNR + " - " + ex + " ----- " + erro);
                        }
                    }
                }
                catch (Exception exx)
                { }
                throw new Exception(ex.ToString());
            }
            try
            {
                SCOService.Job job = new SCOService.Job();
                job.RunbookId = runbookId;

                job.Parameters = System.Web.HttpUtility.UrlDecode(parametersXml.ToString());

                context.AddToJobs(job);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                string strPathFile = @"C:\temp\erro.txt";
                try
                {
                    using (FileStream fs = File.Create(strPathFile))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            string erro = "";
                            foreach (var param in runbookParams)
                            {
                                erro += parameterValues[param.Name];
                            }

                            sw.WriteLine(PERNR + " - " + ex + " ----- " + erro);
                        }
                    }
                }
                catch (Exception exx)
                { }
                throw new Exception(ex.ToString());
            }
        }
    }
}
