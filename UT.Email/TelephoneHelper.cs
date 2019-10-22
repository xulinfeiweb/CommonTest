using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UT.Send
{
    public class TelephoneHelper : ISendable
    {
        public void Send(string message)
        {
            Console.Write("Frome telephone: " + message);
        }
    }
}
