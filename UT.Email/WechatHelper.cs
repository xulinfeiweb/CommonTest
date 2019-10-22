using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UT.Send
{
    public class WechatHelper : ISendable
    {
        public void Send(string message)
        {
            Console.WriteLine("Frome wechat: " + message);
        }
    }
}
