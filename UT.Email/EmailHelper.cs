using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UT.Send
{
    public class EmailHelper : ISendable
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            Console.WriteLine("Frome email: " + message);
        }
    }
}
