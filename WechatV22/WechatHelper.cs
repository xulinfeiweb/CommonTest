using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UT.Send;

namespace UT.WechatV22
{
    public class WechatHelper : ISendable
    {
        public void Send(string message)
        {
            Console.WriteLine("Frome wechatV22: " + message);
        }
    }
}
