using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    public class ServerSetting
    {
        public string user;
        public string pass;
        public string domain;
        public string LogFolderPath;
        public string clientCode;
        public string authCode;

        public void GetSettings(string path)
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                domain = streamReader.ReadLine();
                user = streamReader.ReadLine();
                pass = streamReader.ReadLine();
                LogFolderPath = streamReader.ReadLine();
                clientCode = streamReader.ReadLine();
                authCode = streamReader.ReadLine();
                UTILITIES.logpath = LogFolderPath;
            }
        }
    }
}
