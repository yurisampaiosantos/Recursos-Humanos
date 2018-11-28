using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Configuration;
using BUSINESS;
using DAL;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Principal;


namespace BUSINESS
{
    /// <summary>
    /// Classe negócio do objeto Hollerith.
    /// Responsável por todas as ações necessárias 
    /// para manipulação de objetos do tipo hollerith 
    /// que a classe de visualização precisar.
    /// </summary>
    public class HollerithBLL
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public HollerithBLL()
        {
        }

        /// <summary>
        /// Declaração de lista de objetos que são utilizados na manipulação dos dados.
        /// </summary>
        private List<Hollerith> hollerithDem;
        private List<HollerithFer> hollerithFer;
        private List<HollerithInf> hollerithInf;

        /// <summary>
        /// Retorna os últimos seis demonstrativos disponíveis do usuário.
        /// Consulta realizada no schema HUMANRESOURCES na tabela DM_FILES.
        /// </summary>
        /// <param name="sap_number">Matricula do integrante no SAP</param>
        /// <returns>Lista de objetos Hollerith de demonstrativos</returns>
        public List<Hollerith> getLastSixHollerithDem(string sap_number)
        {
            HollerithDAL hollerithDal = new HollerithDAL();
            this.hollerithDem = hollerithDal.getLastSixHollerithDEM(sap_number);

            return this.hollerithDem;
        }

        /// <summary>
        /// Retorna os últimos seis recibos de férias disponíveis do usuário.
        /// Consulta realizada no schema HUMANRESOURCES na tabela DM_FILES.
        /// </summary>
        /// <param name="sap_number">Matricula do integrante no SAP</param>
        /// <returns>Lista de objetos Hollerith de férias</returns>
        public List<HollerithFer> getLastSixHollerithFer(string sap_number)
        {
            HollerithDAL hollerithDal = new HollerithDAL();
            this.hollerithFer = hollerithDal.getLastSixHollerithFER(sap_number);

            return this.hollerithFer;
        }

        /// Retorna os últimos seis informe de rendimento disponíveis do usuário.
        /// Consulta realizada no schema HUMANRESOURCES na tabela DM_FILES.
        /// </summary>
        /// <param name="sap_number">Matricula do integrante no SAP</param>
        /// <returns>Lista de objetos Hollerith de informe de rendimento</returns>
        public List<HollerithInf> getLastSixHollerithInf(string sap_number)
        {
            HollerithDAL hollerithDal = new HollerithDAL();
            this.hollerithInf = hollerithDal.getLastSixHollerithINF(sap_number);

            return this.hollerithInf;
        }

        /// <summary>
        /// Decriptografa o arquivo na pasta outbound do servidor  e retorna o caminho do mesmo.
        /// Utiliza o path do usuário que se encontra na tabela DM_FILES do schema HUMANRESOURCES.
        /// </summary>
        /// <param name="sap_number">Matricula do integrante no SAP</param>
        /// <param name="path">Caminho do arquivo no banco (DM_FILES)</param>
        /// <returns>Caminho do arquivo decriptografado</returns>
        public string getPathDecriptHollerith(string sap_number, string path)
        {

            try
            {


                WsGetPathDecriptHollerith.WebserviceOpenPdfSap WsGet = new WsGetPathDecriptHollerith.WebserviceOpenPdfSap();

                string returnPath = WsGet.openPdf(sap_number, path);
                //string x = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;// ConfigurationManager.AppSettings.ToString();

                string pathPrinter = ConfigurationManager.AppSettings["returnPath"].ToString() + returnPath.Substring(32, (returnPath.Length - 32));

                return pathPrinter;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        /// <summary>
        /// Move o arquivo decriptografado na pasta outbound para a máquina local.
        /// Utiliza parâmetro do app.config para acessar as pastas.
        /// Colocamos o login e senha no codigo para não ficar exposto no app.config
        /// </summary>
        /// <param name="pathFile">Caminho do arquivo decriptografado</param>
        /// <returns>Caminho na máquina local</returns>
        public string getFileServerToLocal(string pathFile)
        {
            //codigo comentado em 27/10/2013 por Augusto Aragão.
            //Refatorar encontrando solução para não expor logine senha no app.config.
          //  string login = ConfigurationManager.AppSettings["usr"].ToString();
           // string password = ConfigurationManager.AppSettings["pass"].ToString();
            string pathFileLocal = "";
            
            //Faz autenticação no dominio para acesso aos arquivos
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + "eepsa", "F_GL_SAP_HR_DM", "Sppe$#20@13Xx!");
            object nativeObject = entry.NativeObject;
            
            FileInfo filePrint = new FileInfo(pathFile);
            string pathLocal = ConfigurationManager.AppSettings["pathLocal"].ToString();

            //Autenticação para acesso a pasta utilizando o usuário funcional
            IntPtr token;

            if (!NativeMethods.LogonUser(
                "F_GL_SAP_HR_DM",
                "eepsa",
                "Sppe$#20@13Xx!",
                NativeMethods.LogonType.NewCredentials,
                NativeMethods.LogonProvider.Default,
                out token))
            {
                throw new Win32Exception();
            }

            try
            {
                IntPtr tokenDuplicate;

                if (!NativeMethods.DuplicateToken(
                    token,
                    NativeMethods.SecurityImpersonationLevel.Impersonation,
                    out tokenDuplicate))
                {
                    throw new Win32Exception();
                }

                try
                {
                    using (WindowsImpersonationContext impersonationContext =
                        new WindowsIdentity(tokenDuplicate).Impersonate())
                    {
                        //Faz a cópia do arquivo para o servidor local
                        if (!(File.Exists(Path.Combine(pathLocal, filePrint.Name))))
                            filePrint.CopyTo(Path.Combine(pathLocal, filePrint.Name));

                        pathFileLocal = Path.Combine(pathLocal, filePrint.Name);
                        impersonationContext.Undo();

                        return pathFileLocal;
                        
                    }
                }
                finally
                {
                    if (tokenDuplicate != IntPtr.Zero)
                    {
                        if (!NativeMethods.CloseHandle(tokenDuplicate))
                        {
                            // Uncomment if you need to know this case.
                            ////throw new Win32Exception();
                        }
                    }
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    if (!NativeMethods.CloseHandle(token))
                    {
                        // Uncomment if you need to know this case.
                        ////throw new Win32Exception();
                    }
                }
            }
        }

        /// <summary>
        /// Recupera o caminho do arquivo decriptografado para impressão.
        /// Utiliza o webservice para decriptografia do arquivo
        /// </summary>
        /// <param name="sap_number">Matricula do integrante no SAP</param>
        /// <param name="path">Caminho do arquivo</param>
        /// <returns>Caminho do arquivo decriptografado</returns>
        public string getPathFilePrint(string sap_number, string path)
        {
            try
            {
                WsGetPathDecriptHollerith.WebserviceOpenPdfSap WsGet = new WsGetPathDecriptHollerith.WebserviceOpenPdfSap();
                string returnPath = WsGet.printFileTotem(sap_number, path);

                return returnPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
