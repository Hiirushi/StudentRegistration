using StudentRegistration.Data;
using StudentRegistration.Repositories;
using StudentRegistration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentRegistration
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DbConnectionFactory dbConnectionFactory =
                new DbConnectionFactory();

            StudentRepository studentRepository =
                new StudentRepository(dbConnectionFactory);

            StudentService studentService =
                new StudentService(studentRepository);

            CourseRespository courseRespository =
                new CourseRespository(dbConnectionFactory);

            Application.Run(
                 new StudentRegistration(
                    studentService,
                    studentRepository,
                    courseRespository
                )
            );
        }
    }
}
