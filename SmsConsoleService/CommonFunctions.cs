using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    internal class CommonFunctions
    {
        public static void WriteLog(string logText)
        {
            try
            {
                DateTime now = DateTime.Now;
                string path = "C:\\LOGS\\SmsConsoleService_logs";
                string str1 = "log_" + now.ToString("yyyy-MM-dd HH") + ".txt";
                string str2 = path + "\\" + str1;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!File.Exists(str2))
                    File.Create(str2);
                CommonFunctions.ProcessWrite(str2, logText + "\n").Wait();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private static Task ProcessWrite(string filePath, string logText) => CommonFunctions.WriteTextAsync(filePath, logText);

        private static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, true))
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            encodedText = (byte[])null;
        }
    }
}
