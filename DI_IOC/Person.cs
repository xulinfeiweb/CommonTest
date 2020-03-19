using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_IOC
{
    public class Person
    {
        private string name;
        private int age;
        private Phone6 phone;
        public Person(string name, int age, Phone6 phone)
        {
            this.name = name;
            this.age = age;
            this.phone = phone;
        }
        public string Read(string str)
        {
            return str + "123";
        }
    }
}
