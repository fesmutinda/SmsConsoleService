using System.ServiceProcess;

namespace SmsConsoleService
{
    partial class Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceInstaller1 = new ServiceInstaller();
            this.serviceProcessInstaller1 = new ServiceProcessInstaller();
            this.serviceInstaller1.DelayedAutoStart = true;
            this.serviceInstaller1.ServiceName = "SmsConsole";
            this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
            this.serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = (string)null;
            this.serviceProcessInstaller1.Username = (string)null;
            this.Installers.AddRange(new System.Configuration.Install.Installer[2]
            {
        (System.Configuration.Install.Installer) this.serviceInstaller1,
        (System.Configuration.Install.Installer) this.serviceProcessInstaller1
            });
        }

        #endregion
    }
}