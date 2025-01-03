using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    internal class Program
    {
        private static SmsConsole _service;
        private static void Main(string[] args)
        {
            if (args != null && args.Length == 1 && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    case "install":
                    case "i":
                        if (!ServiceInstallerUtility.InstallMe())
                        {
                            CommonFunctions.WriteLog(string.Format("{0}\tFailed to install service", DateTime.Now));
                            break;
                        }
                        break;
                    case "uninstall":
                    case "u":
                        if (!ServiceInstallerUtility.UninstallMe())
                        {
                            CommonFunctions.WriteLog(string.Format("{0}\tFailed to uninstall service", DateTime.Now));
                            break;
                        }
                        break;
                    default:
                        CommonFunctions.WriteLog(string.Format("{0}\tUnrecognized parameters (allowed: /install and /uninstall, shorten /i and /u)", DateTime.Now));
                        break;
                }
                Environment.Exit(0);
            }

            _service = new SmsConsole();
            ServiceBase[] services = new ServiceBase[1]
            {
                _service
            };

            if (Environment.UserInteractive)
            {
                CommonFunctions.WriteLog(string.Format("{0}\tRunning in console mode", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
                _service.Start();
                Console.WriteLine("Press any key to stop the service...");
                Console.Read();
                _service.Stop();
            }
            else
                ServiceBase.Run(services);

            while (true)
            {
                try
                {
                    SmsEngine smsEngine = new SmsEngine();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" error on main " + ex.Message + "\t\t" + ex.InnerException?.ToString());
                }
            }
        }
    }
}
