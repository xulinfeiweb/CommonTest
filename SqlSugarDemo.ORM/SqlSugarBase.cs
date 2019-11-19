using SqlSugar;
using System;
using System.Linq;

namespace SqlSugarDemo.ORM
{
    /// <summary>
    /// 数据库 上下文
    /// </summary>
    public class SqlSugarBase
    {
        public SqlSugarClient DB => GetInstance();

        SqlSugarClient GetInstance()
        {
            string connectionString = "Server=.;Database=SqlSugarDemo;Integrated Security=False;User ID=sa;Password=123456";

            var db = new SqlSugarClient(
                new ConnectionConfig
                {
                    ConnectionString = connectionString,
                    DbType = DbType.SqlServer,
                    IsShardSameThread = true,
                    InitKeyType = InitKeyType.Attribute
                }
            );
            return db;
        }
    }
}
