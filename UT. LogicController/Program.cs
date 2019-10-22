using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UT.MessageService;
using UT.Send;

namespace UT.LogicController
{
    class Program
    {
        static void Main(string[] args)
        {
            //string message = "新年快乐！过节费5000.";
            #region 版本0.0.1
            //GreetMessageService service = null;
            ////UT公司（微信方式）
            //ISendable wechat = new WechatHelper();
            //service = new GreetMessageService(wechat);
            //service.Greet(message);
            ////UT编辑部（短信方式） 
            //ISendable sms = new SMSHelper();
            //service = new GreetMessageService(sms);
            //service.Greet(message);
            ////UT房产(邮件方式)
            //ISendable mail = new EmailHelper();
            //service = new GreetMessageService(mail);
            //service.Greet(message);
            #endregion
            #region 版本0.0.2
            //采用工厂模式创建实例
            //ISendable greetTool = SendToolFactory.GetInstance();
            //GreetMessageService service = new GreetMessageService(greetTool);
            //service.Greet(message);
            #endregion
            //string str = DateTime.Now.AddDays(-1 * count).ToString("yyyy-MM-dd");

            //Student st = new Student();
            //Student st2 = new Student("lily", new DateTime(2003, 5, 28));
            //goodStudent goodst = new goodStudent();
            //goodStudent goodst2 = new goodStudent("harfla");
            //goodStudent goodst3 = new goodStudent("david", new DateTime(1990, 6, 18), "Tqing");
            //Console.WriteLine("test:" + st.Name);
            //Console.ReadLine();
            int? getNum = null;
            Console.WriteLine(getNum ?? 0);
            Console.ReadLine();
        }
        public class Student
        {
            protected int Age;
            public string Name;
            public DateTime birthDate;
            public int GetAge()
            {
                int ts = DateTime.Now.Year - birthDate.Year;
                return ts;
            }
            public Student()
            {
                Console.WriteLine("默认的构造函数");
            }
            public Student(string name, DateTime birthDt)
            {
                this.Name = name;
                this.birthDate = birthDt;
                Console.WriteLine("带参数构造函数");
                Console.WriteLine("name:{0}--birthdate:{1}", Name, birthDate.ToString("yyyy-MM-dd"));
            }
        }
        public class goodStudent : Student
        {
            public int Score;
            public string University;
            public goodStudent() : base()
            { }
            public goodStudent(string name, DateTime birthDat, string school) : base(name, birthDat)
            {
                this.Age = DateTime.Now.Year - birthDate.Year;
                this.University = school;
                Console.WriteLine("name:{0}--birth:{1}--age:{2}", Name, birthDate.ToString("yyyy-MM-dd"), Age);
                Console.WriteLine("university--{0}", University);
            }
            public goodStudent(string uv) : base()
            {
                this.University = uv;
                Console.WriteLine("unviersity:{0}", uv);
            }
        }
    }
}
