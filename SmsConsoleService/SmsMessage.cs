using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    public class SmsMessage
    {
        public MessageList[] messageList { get; set; }

        public class MessageList
        {
            public string msisdn { get; set; }
            public string refnum {  get; set; }
            public string smsmessage { get; set; }
            public string clientcode { get; set; }
            public string authcode { get; set; }
            public string msgsource { get; set; }
        }
    }
}
