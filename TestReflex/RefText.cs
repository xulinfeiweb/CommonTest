using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReflex
{
    public class RefText : Fbase
    {
        public int age;

        public RefText(int age)
        {
            this.age = age;
        }
        public string Reads(DataTable dt, Dictionary<string, string> dic)
        {
            string result = name + "今年" + age + "岁了，但是还是单身，真替他着急。";
            return result;
        }
    }
}
