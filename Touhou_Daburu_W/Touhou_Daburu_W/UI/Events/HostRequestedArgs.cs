using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touhou_Daburu_W.UI.Events
{
    public class HostRequestedArgs : EventArgs
    {
        public HostRequestedArgs(string port) { Port = port; }
        public string Port { get; }
    }
}
