using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private ServiceInstaller serviceInstaller1;
        private ServiceProcessInstaller serviceProcessInstaller1;

        public Installer()
        {
            InitializeComponent();
        }

        private string ServiceName
        {
            get
            {
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                return friendlyName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? friendlyName.Substring(0, friendlyName.Length - 4) : friendlyName;
            }
        }

        private string DisplayName => ServiceName;

        private string Description => ServiceName;

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
            serviceInstaller1.ServiceName = ServiceName;
            serviceInstaller1.DisplayName = DisplayName;
            serviceInstaller1.Description = Description;
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            serviceInstaller1.ServiceName = ServiceName;
            serviceInstaller1.DisplayName = DisplayName;
            serviceInstaller1.Description = Description;
        }
    }
}
