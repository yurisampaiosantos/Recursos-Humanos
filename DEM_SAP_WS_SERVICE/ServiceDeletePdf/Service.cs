using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace ServiceDeletePdf
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        //Definição do nome do serviço e descrição do serviço.
        public const string NAME = "ServiceDeletePdf";
        public const string DISPLAY_NAME = "Service Delete Pdf (Temp)";


        #region Instalação do serviço
        static void Main(string[] args)
        {
            try
            {
                Trace.WriteLine("Iniciando DEBUG");

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }


            if (!Environment.UserInteractive)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new Service() 
            };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {

                ServiceController sc = new ServiceController(NAME);


                if (!ServiceExists())
                {
                    if (DialogResult.OK == MessageBox.Show("Deseja instalar o serviço " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        try
                        {
                            Trace.WriteLine("Instalando o serviço \"" + DISPLAY_NAME + "\"...");
                            ProjectInstaller.Install();
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(ex.Message);
                        }
                    }
                }
                else
                {
                    if (DialogResult.OK == MessageBox.Show("Deseja desinstalar o serviço " + DISPLAY_NAME + "?", DISPLAY_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        try
                        {
                            Trace.WriteLine("Desinstalando o serviço \"" + DISPLAY_NAME + "\"...");
                            ProjectInstaller.Uninstall();
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(ex.Message);
                        }
                    }
                }
            }
        }
        #endregion

        //Objeto de tempo para execução do serviço.
        System.Timers.Timer timer = new System.Timers.Timer();


        //Método de inicialização do serviço e definição de intervalo de execução.
        protected override void OnStart(string[] args)
        {
            timer.Interval = 5000;
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;

        }

        protected override void OnStop()
        {
        }

        //Método que verifica se o serviço está instalado
        private static bool ServiceExists()
        {
            foreach (ServiceController sc in ServiceController.GetServices())
                if (sc.ServiceName == NAME)
                    return true;
            return false;
        }

        //Método de log e execução do serviço.
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            EventLog.WriteEntry("Executando serviço: " +
            DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
            DeletePdf.Start();
        }


    }
}
