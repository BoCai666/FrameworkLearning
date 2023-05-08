using System;
using System.Collections.Generic;
using System.Text;

namespace IOC.UnitTest
{
    class TestConstructorInjection : IUnitTest
    {
        public void Test()
        {
            var c = new Container();
            c.ConfigureInstanceProvider(new RelfectionInstanceProvider(c));
            c.Register<IStudent, Student>();
            c.Register<ITeacher, Teacher>();
            c.Register<IClassRoom, ClassRoom>();
            var t = c.Resolve<IClassRoom>().GetTeacher();
            Console.WriteLine($"IsNull: {t == null} t: {t}");
        }

        interface IStudent { }
        interface ITeacher
        {
            IStudent GetStudent();
        }
        interface IClassRoom
        {
            ITeacher GetTeacher();
        }
        class Student : IStudent { }
        class Teacher : ITeacher
        {
            public IStudent student;

            public Teacher(IStudent student)
            {
                this.student = student;
            }

            public IStudent GetStudent()
            {
                return student;
            }
        }
        class ClassRoom : IClassRoom
        {
            private Teacher teacher;
            private IStudent student;

            [Inject]
            public ClassRoom(Teacher teacher) 
            { 
                this.teacher = teacher;
            }
            public ClassRoom(Teacher teacher, IStudent student)
            {
                this.teacher = teacher;
                this.student = student;
            }
            public ITeacher GetTeacher() => teacher;
        }
    }
}
