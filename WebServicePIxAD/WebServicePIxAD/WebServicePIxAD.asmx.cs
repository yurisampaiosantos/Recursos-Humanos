using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServicePIxAD
{
    /// <summary>
    /// Replica as informações do PI para o Orchestrator
    /// </summary>    
    [WebService(Namespace = "http://www.eepsa.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
   
    public class WebServicePIxAD : System.Web.Services.WebService
    {
        /// <summary>
        /// Metodo que recebe do PI os dados do SAP e envia para o Orchestrator   
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
        /// 
        /// 
        /// <returns>Verdadeiro(TRUE) ou Falso(FALSE)</returns>
        
        [WebMethod(Description = "Replica as informações para o orchestrator 2012", EnableSession = true)]
        public string CreateUserAD(string PERNR, string MASSN, string BEGDA, string CPF_NR, string CNAME, string BUKRS, string BUTXT, string ORGEH, string ORGTX, string KOSTL, string KONSLTX,
                                   string BTRTL, string BTEXT, string KONSL, string RH_GET_MANAGER_ASSIGNMENT, string STELL, string STEXT, string PERSK, string PTEXT, string GBDAT, string MASSG, string LOGIN_SUGERIDO,
                                   string NIVEL_BAS, string NIVEL_INT, string NIVEL_AVA, string LTEXT, string NAME_TEXT)
        {
            try
            {
                //Instacia o objeto AD
                ConnectAD.AD ad = new ConnectAD.AD();
                //convete pra data
                string begda = "";
                try
                {
                    begda = BEGDA.Substring(4, 2) + "/" + BEGDA.Substring(6, 2) + "/" + BEGDA.Substring(0, 4);
                }
                catch { }

                //captura o ano da data de nascimento
                string data = "";
                try
                {
                    data = GBDAT.Substring(0, 4);
                }
                catch { }
                string orgtx = "";
                try
                {
                    orgtx = ORGTX.Replace("&", "E");
                }
                catch
                { orgtx = ORGTX; }

                string butxt = "";
                try
                {
                    butxt = BUTXT.Replace("&", "E");
                }
                catch
                { butxt = BUTXT; }

                string stext = "";
                try
                {
                    stext = STEXT.Replace("&", "E");
                }
                catch
                { stext = STEXT; }

                string ptext = "";
                try
                {
                    ptext = PTEXT.Replace("&", "E");
                }
                catch
                { ptext = PTEXT; }

                string konsltx = "";
                try
                {
                    konsltx = KONSLTX.Replace("&", "E");
                }
                catch
                { konsltx = KONSLTX; }

                string massg = "";
                try
                {
                    massg = MASSG.Replace("&", "E");
                }
                catch
                { massg = MASSG; }
                
                //Envia para o orchestrator
                ad.Send(PERNR, MASSN, begda, CPF_NR, CNAME, BUKRS, butxt, ORGEH, orgtx, KOSTL, konsltx,
                          BTRTL, BTEXT, KONSL, RH_GET_MANAGER_ASSIGNMENT, STELL, stext, PERSK, ptext, data, massg, LOGIN_SUGERIDO,
                          NIVEL_BAS, NIVEL_INT, NIVEL_AVA, LTEXT, NAME_TEXT);
                return "T" + PERNR;
            }
            catch(Exception ex)
            {
                //Se apresentar algum problema ao enviar pro Orchestrator
                return "F" + PERNR;// ex.Message;
            }
            
        }
    }
}
