using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touhou_Daburu_W.UI.Events
{
    class ConnectRequestArgs
    {
        public ConnectRequestArgs(string ip, string port) { Ip = ip; Port = port; }
        public string Ip { get; }
        public string Port { get; }
    }
}
