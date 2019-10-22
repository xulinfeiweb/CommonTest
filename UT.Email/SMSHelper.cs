using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UT.Send
{
    public class SMSHelper : ISendable
    {
        public void Send(string message)
        {
            Console.WriteLine("Frome SMS: " + message);
        }
    }
}
