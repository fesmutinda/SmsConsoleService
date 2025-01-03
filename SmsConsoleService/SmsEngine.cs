using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using SmsConsoleService.NAVWEBREF;
using SmsConsoleService.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Linq;
using static SmsConsoleService.SmsMessage;

namespace SmsConsoleService
{
    public class SmsEngine
    {
        private smsReference.SURESMSSERVICE service = new smsReference.SURESMSSERVICE();    
        private SURESMSSERVICE navsms = new SURESMSSERVICE();
        private ServerSetting ss = new ServerSetting();

        public CloudPESAMB.CPMOBILE mobileService = new CloudPESAMB.CPMOBILE();
        public SmsEngine()
        {
            ss.GetSettings(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Settings.txt");
            NetworkCredential cd = new NetworkCredential(ss.user, ss.pass, ss.domain);
            navsms.Url = "http://192.168.3.2:7047/BC230/WS/MAFANIKIO%20SACCO/Codeunit/SURESMSSERVICE";
            navsms.UseDefaultCredentials = true;

            mobileService = new CloudPESAMB.CPMOBILE();
            mobileService.Credentials = new NetworkCredential(ss.user, ss.pass, ss.domain);

            service = new smsReference.SURESMSSERVICE();
            //service.Url = "http://192.168.3.2:7047/BC230/WS/MAFANIKIO%20SACCO/Codeunit/SURESMSSERVICE";
            service.Url = "http://192.168.3.2:7047/BC230/WS/DEVELOPMENT/Codeunit/SURESMSSERVICE";
            service.Credentials = new NetworkCredential(ss.user, ss.pass, ss.domain);
        }

        public string RunService()//RunSmses()
        {
            string response = string.Empty;

            try
            {
                Console.WriteLine("===> Connecting to fetch SMS from CBS ... ");
                
                string odataEndpoint = "http://192.168.3.2:7048/BC230/ODataV4/Company('MAFANIKIO%20SACCO')/SmsMessages";
                
                string xmlData = GetDataFromOData(odataEndpoint);

                if (!string.IsNullOrEmpty(xmlData))
                {
                    response = ParseXmlData(xmlData);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve data from the OData service.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed. Error occurred at : {ex.Message}");
            }
            return response;
        }
        public void RunSmses()
        {
            Console.WriteLine("===> Connecting to fetch SMS from CBS ... ");
            try
            {

                var item = service.PollPendingSMS();
                if (!string.IsNullOrEmpty(item))
                {
                    string[] ministmtArray = item.Split(new string[] { ":::" }, StringSplitOptions.None);

                    MessageList message = new MessageList
                    {
                        msisdn = ministmtArray[0],
                        refnum = ministmtArray[2],
                        smsmessage = ministmtArray[1],
                        clientcode = ss.clientCode,
                        authcode = ss.authCode,
                        msgsource = "784"
                    };

                    if (message != null && ministmtArray[2] == "139")
                    {
                        string jsonContent = JsonConvert.SerializeObject(message);

                        Task<bool> response = SendMessage(jsonContent);

                        if (response.Result)
                        {
                            bool smsSent = service.ConfirmSent(ministmtArray[0], int.Parse(ministmtArray[2]));
                            Console.WriteLine("======================================================");
                            Console.WriteLine($"Updated SMS with ==> ID: {ministmtArray[2]} and Telephone No: {ministmtArray[0]}");
                        }
                        else
                        {
                            Console.WriteLine("Failed to send SMS to server");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.Data.Clear();
            }


        }
        public string GetDataFromOData(string endpoint)
        {
            NetworkCredential credential = new NetworkCredential(ss.user, ss.pass, ss.domain);
            using (HttpClientHandler handler = new HttpClientHandler { Credentials = credential })
            //using (HttpClientHandler handler = new HttpClientHandler {  UseDefaultCredentials = true })
            using (HttpClient httpClient = new HttpClient(handler))
            {

                HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }


        public string ParseXmlData(string xmlData)
        {
            string result = string.Empty;

            try
            {
                XDocument xDocument = XDocument.Parse(xmlData);
                XNamespace nsAtom = "http://www.w3.org/2005/Atom";
                XNamespace nsData = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
                XNamespace nsProp = "http://schemas.microsoft.com/ado/2007/08/dataservices";

                var entries = xDocument.Descendants(nsAtom + "entry");

                foreach (var entry in entries)
                {
                    var entryId = entry.Element(nsAtom + "id")?.Value;
                    var content = entry.Element(nsAtom + "content");
                    if (content != null)
                    {
                        var properties = content?.Element(nsData + "properties");

                        if (properties != null)
                        {
                            var entryNo = properties?.Element(nsProp + "Entry_No")?.Value;
                            var telephoneNo = properties?.Element(nsProp + "Telephone_No")?.Value;
                            var smsMessage = properties?.Element(nsProp + "SMS_Message")?.Value;
                            var sentToServer = properties?.Element(nsProp + "Sent_To_Server")?.Value;

                            Console.WriteLine("======================================================");
                            Console.WriteLine($"Message details to send ==> Entry No: {entryNo}, Telephone No: {telephoneNo}, SMS Message: {smsMessage}");

                            MessageList message = new MessageList
                            {
                                msisdn = telephoneNo,
                                refnum = entryNo,
                                smsmessage = smsMessage,
                                clientcode = ss.clientCode,
                                authcode = ss.authCode,
                                msgsource = "784"
                            };

                            if (message != null)
                            {
                                if (telephoneNo.Equals("+254743901110"))
                                {
                                    string jsonContent = JsonConvert.SerializeObject(message);

                                    Task<bool> response = SendMessage(jsonContent);

                                    if (response.Result)
                                    {
                                        bool smsSent = navsms.UpdateConfirmSent(int.Parse(entryNo));
                                        Console.WriteLine("======================================================");
                                        Console.WriteLine($"Updated SMS with ==> ID: {entryNo} and Telephone No: {telephoneNo}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to send SMS to server");
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed. Error occurred at : {ex.Message}");
            }
            return result;
        }

        public async Task<bool> SendMessage(string messageContent)
        {
            bool result = false;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://suresms.co.ke:45322/messenger/send");
                var content = new StringContent(messageContent, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
                else
                    result = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed. Error occurred at : {ex.Message}");
            }
            return result;
        }
    }
}
