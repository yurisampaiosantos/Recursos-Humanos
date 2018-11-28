using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace ServiceServerAux
{
    [RunInstaller(true)]
    public partial class ProjectInstallerAux : System.Configuration.Install.Installer
    {
        public ProjectInstallerAux()
        {
            ServiceProcessInstaller spi = new ServiceProcessInstaller();
            spi.Account = ServiceAccount.LocalSystem;
            System.ServiceProcess.ServiceInstaller si = new System.ServiceProcess.ServiceInstaller();
            si.ServiceName = ServiceAux.NAME;
            si.DisplayName = ServiceAux.DISPLAY_NAME;
            si.Description = ServiceAux.DISPLAY_NAME;
            si.StartType = ServiceStartMode.Automatic;

            Installers.Add(spi);
            Installers.Add(si);
        }

        public static void Install()
        {
            string[] s = { Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + ServiceAux.NAME + ".exe" };
            ManagedInstallerClass.InstallHelper(s);
            ServiceController sc = new ServiceController(ServiceAux.NAME);
            sc.Start();
        }

        public static void Uninstall()
        {
            string[] s = { "/u", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + ServiceAux.NAME + ".exe" };
            ManagedInstallerClass.InstallHelper(s);
        }

    }
}
