using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEE
{
    class ConnectionDetails
    {
        public static string username { get; set; }
        public static string password { get; set; }
        public static string roomID { get; set; }

        public static Object _lock = new Object();
    }
}
