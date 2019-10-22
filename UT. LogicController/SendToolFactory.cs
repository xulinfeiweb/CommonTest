using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UT.Send;

namespace UT.LogicController
{
    public abstract class SendToolFactory
    {
        public static ISendable GetInstance()
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(GetAssembly());
                object obj = assembly.CreateInstance(GetObjectType());
                return obj as ISendable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        static string GetAssembly()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["AssemblyString"]);
        }
        static string GetObjectType()
        {
            return ConfigurationManager.AppSettings["TypeString"];
        }
    }
}
