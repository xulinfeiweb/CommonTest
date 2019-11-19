using Models;
using SqlSugarDemo.ORM;
using System;
using System.Collections.Generic;

namespace SqlSugarDemo.Service
{
    public class HomeService : SqlSugarBase
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public List<Student> GetList()
        {
            return DB.Queryable<Student>().ToList();
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            Student model = new Student();
            model.StuID = 8;
            model.Name = "abc";
            model.CourseId = 2;
            var test = DB.Insertable(model).ExecuteCommand();
            return test > 0;
        }
    }
}
