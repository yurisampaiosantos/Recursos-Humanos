using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace ServiceAuxAPP02
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
       public ProjectInstaller()
        {
            
            ServiceProcessInstaller spi = new ServiceProcessInstaller();
            spi.Account = ServiceAccount.LocalSystem;
            System.ServiceProcess.ServiceInstaller si = new System.ServiceProcess.ServiceInstaller();
            si.ServiceName = ServiceAuxAPP02.NAME;
            si.DisplayName = ServiceAuxAPP02.DISPLAY_NAME;
            si.Description = ServiceAuxAPP02.DISPLAY_NAME;
            si.StartType = ServiceStartMode.Automatic;

            Installers.Add(spi);
            Installers.Add(si);
        }

        public static void Install()
        {
            string[] s = { Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + ServiceAuxAPP02.NAME + ".exe" };
            ManagedInstallerClass.InstallHelper(s);
            ServiceController sc = new ServiceController(ServiceAuxAPP02.NAME);
            sc.Start();
        }

        public static void Uninstall()
        {
            string[] s = { "/u", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + ServiceAuxAPP02.NAME + ".exe" };
            ManagedInstallerClass.InstallHelper(s);
        }


        }
    
}
