using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace LabTechnicianWindowService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.AfterInstall += new InstallEventHandler(ServiceInstaller_AfterInstall);
        }

        void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            using (ServiceController sc = new ServiceController(this.serviceInstaller1.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
