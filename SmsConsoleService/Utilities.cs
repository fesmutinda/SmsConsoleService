using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    internal class UTILITIES
    {
        private static Collection<string> logs = new Collection<string>();
        private static ServerSetting ss = new ServerSetting();
        public static string logpath = "C:\\LOGS\\Sms Logs\\SmsService_logs";

        public static string LogFileName
        {
            get
            {
                if (!Directory.Exists(ss.LogFolderPath))
                    Directory.CreateDirectory(logpath);
                return UTILITIES.logpath + DateTime.Now.ToString("yyyy-MMM-dd") + ".txt";
            }
        }

        private UTILITIES()
        {
        }

        public static void WriteLog(string clientRequest)
        {
            try
            {
                File.AppendAllText(UTILITIES.LogFileName, clientRequest + "\n");
            }
            catch
            {
            }
        }
    }
}
