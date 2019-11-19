using Models;
using SqlSugarDemo.ORM;
using System;
using System.Collections.Generic;

namespace SqlSugarDemo.Service
{
    public class HomeService : SqlSugarBase
    {
        public List<Student> GetList()
        {
            return DB.Queryable<Student>().ToList();
        }
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
