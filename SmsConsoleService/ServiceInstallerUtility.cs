using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    internal class ServiceInstallerUtility
    {
        private static readonly string exePath = Assembly.GetExecutingAssembly().Location;

        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(args: new string[2] { "/LogFile=", exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(args: new string[3] { "/LogFile=", "/u", exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
