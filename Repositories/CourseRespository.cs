using Microsoft.Data.SqlClient;
using StudentRegistration.Data;
using System.Data;

namespace StudentRegistration.Repositories
{
    public class CourseRespository
    {
        private readonly DbConnectionFactory dbConnectionFactory;

        public CourseRespository(DbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;   
        }

        public DataTable GetAllCourses()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = dbConnectionFactory.CreateConnection())
            {
                string query = "SELECT CourseID, CourseName FROM Courses Order By CourseName";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(table);
                }
            }
            return table;
        }
    }
}
