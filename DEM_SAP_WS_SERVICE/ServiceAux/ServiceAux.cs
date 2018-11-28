using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ServiceAux
{
    partial class ServiceAux : ServiceBase
    {
        public ServiceAux()
        {
            ConsoleTraceListener listener = new ConsoleTraceListener();
            Trace.Listeners.Add(listener);
            InitializeComponent();
        }

        public const string NAME = "ServiceAux";
        public const string DISPLAY_NAME = "Service Move APP01";

        #region Instalação do serviço
        //Instalação do serviço
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
                new ServiceAux() 
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
                            ProjectInstallerAux.Install();
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
                            ProjectInstallerAux.Uninstall();
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

        System.Timers.Timer timer = new System.Timers.Timer();

        protected override void OnStart(string[] args)
        {
            timer.Interval = 30000;
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;
        }

        private static bool ServiceExists()
        {
            foreach (ServiceController sc in ServiceController.GetServices())
                if (sc.ServiceName == NAME)
                    return true;
            return false;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            string Processo = Process.GetCurrentProcess().ProcessName;

            EventLog.WriteEntry("Executando serviço: " + Processo +
            DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);


            MoveFile.Start();


        }



        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
