using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UT.Send;

namespace UT.MessageService
{
    public class GreetMessageService
    {
        ISendable greetTool;
        public GreetMessageService(ISendable greettool)
        {
            this.greetTool = greettool;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message"></param>
        public void Greet(string message)
        {
            greetTool.Send(message);
        }
    }
}
