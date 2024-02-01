using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_1_SQL
{
    public class DataHandler
    {


        public void Fetch_all_Students(SqlConnection connection)
        {
            Console.Write("Do you want to sort on 1. firstname or 2.last name: ");
            string orderInput = Console.ReadLine();
            string sortingColumn = (orderInput == "1") ? "FirstName" : (orderInput == "2") ? "LastName" : "";

            if (string.IsNullOrEmpty(sortingColumn))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Console.Write("Do you want to sort on 1. ASC or 2.DESC: ");
            string ascDescInput = Console.ReadLine();
            string sortingDirection = (ascDescInput == "1") ? "ASC" : (ascDescInput == "2") ? "DESC" : "";

            if (string.IsNullOrEmpty(sortingDirection))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            string queryString = $"SELECT [FirstName], [LastName] FROM [dbo].[Students] ORDER BY {sortingColumn} {sortingDirection};";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                        string _lastname = reader.GetString(reader.GetOrdinal("LastName"));
                        

                        Console.WriteLine($"Firstname: {_firstname} Lastname: {_lastname}");
                    }
                }
            }
        }

        public void Fetch_Students_class(SqlConnection connection)
        {


            using (SqlCommand command = new SqlCommand("SELECT Students.FirstName, Students.LastName\r\nFROM Students\r\nJOIN Classes ON Students.ClassID = Classes.ClassID\r\nWHERE Classes.ClassName = @ClassName;", connection))
            {
                Console.Write("Class to view: ");
                string class_input = Console.ReadLine();
                command.Parameters.AddWithValue("@ClassName", class_input);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                        Console.WriteLine($"Firstname: {_firstname}");
                    }
                }
            }

        }


        public void Add_Teacher(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO Teachers (FirstName, SchoolRole, ClassID)\r\nVALUES (@NewTeacherFirstName, @NewTeacherRole, (SELECT ClassID FROM Classes WHERE ClassName = @TargetClassName));", connection))
            {
                Console.WriteLine("Enter some details of the teahcer");
                Console.Write("Teachername: ");
                string teacherName_input = Console.ReadLine();
                Console.Write("Role: ");
                string teacher_role = Console.ReadLine();
                Console.Write("Class: ");
                string teacher_class = Console.ReadLine();
                command.Parameters.AddWithValue("@NewTeacherFirstName", teacherName_input);
                command.Parameters.AddWithValue("@NewTeacherRole", teacher_role);
                command.Parameters.AddWithValue("@TargetClassName", teacher_class);
                

                command.ExecuteNonQuery();
                Console.WriteLine($"Added {teacherName_input} to the database.");
            }



        }


        public void Fetch_Teacher(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("SELECT FirstName, SchoolRole\r\nFROM Teachers\r\nWHERE SchoolRole = @TargetSchoolRole;", connection))
            {
                Console.Write("Search for a Schoolrole:  ");
                string role_input = Console.ReadLine();
                command.Parameters.AddWithValue("@TargetSchoolRole", role_input);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                        Console.WriteLine($"Firstname: {_firstname}");
                    }
                }
            }

        }

        public void FetchGrades(SqlConnection connection)
        {
            try
            {
                // Assuming you want to get the grades for the previous month
                DateTime startDate = DateTime.Now.AddMonths(-1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                using (SqlCommand command = new SqlCommand(@"
            SELECT
                Students.FirstName AS StudentFirstName,
                Students.LastName AS StudentLastName,
                Courses.CourseName,
                Grades.GradeName,
                Grades.GradeDate
            FROM
                Grades
            JOIN Students ON Grades.StudentID = Students.StudentID
            JOIN Courses ON Grades.CourseID = Courses.CourseID
            WHERE
                Grades.GradeDate >= @StartDate AND Grades.GradeDate <= @EndDate;", connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string studentFirstName = reader.GetString(reader.GetOrdinal("StudentFirstName"));
                            string studentLastName = reader.GetString(reader.GetOrdinal("StudentLastName"));
                            string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                            int gradeName = reader.GetInt32(reader.GetOrdinal("GradeName"));
                            DateTime gradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate"));

                            Console.WriteLine($"Student: {studentFirstName} {studentLastName}, Course: {courseName}, Grade: {gradeName}, Date: {gradeDate}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to log the error or handle it in a way that makes sense for your application.
            }
        }

        public void FetchCourseStatistics(SqlConnection connection)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(@"
            SELECT
                Courses.CourseName,
                AVG(CAST(Grades.GradeName AS FLOAT)) AS AverageScore,
                MAX(Grades.GradeName) AS BestScore,
                MIN(Grades.GradeName) AS LowestScore
            FROM
                Courses
            LEFT JOIN Grades ON Courses.CourseID = Grades.CourseID
            GROUP BY
                Courses.CourseName;", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                            double? averageScore = reader.IsDBNull(reader.GetOrdinal("AverageScore")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("AverageScore"));
                            int? bestScore = reader.IsDBNull(reader.GetOrdinal("BestScore")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("BestScore"));
                            int? lowestScore = reader.IsDBNull(reader.GetOrdinal("LowestScore")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LowestScore"));

                            Console.WriteLine($"Course: {courseName}");
                            Console.WriteLine($"  Average Score: {(averageScore.HasValue ? averageScore.ToString() : "N/A")}");
                            Console.WriteLine($"  Best Score: {(bestScore.HasValue ? bestScore.ToString() : "N/A")}");
                            Console.WriteLine($"  Lowest Score: {(lowestScore.HasValue ? lowestScore.ToString() : "N/A")}");
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to log the error or handle it in a way that makes sense for your application.
            }
        }

        public void AddStudent(SqlConnection connection)
        {
            try
            {
                Console.WriteLine("Enter details for the new student:");
                Console.Write("First Name: ");
                string firstName = Console.ReadLine();
                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();
                Console.Write("Class Name: ");
                string className = Console.ReadLine();
                Console.Write("Course Name: ");
                string courseName = Console.ReadLine();

                // Start a SQL transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert the new student into the Students table
                        using (SqlCommand insertStudentCommand = new SqlCommand(@"
                    INSERT INTO Students (FirstName, LastName, ClassID, CourseID)
                    VALUES (@FirstName, @LastName, (SELECT ClassID FROM Classes WHERE ClassName = @ClassName),
                            (SELECT CourseID FROM Courses WHERE CourseName = @CourseName));", connection, transaction))
                        {
                            insertStudentCommand.Parameters.AddWithValue("@FirstName", firstName);
                            insertStudentCommand.Parameters.AddWithValue("@LastName", lastName);
                            insertStudentCommand.Parameters.AddWithValue("@ClassName", className);
                            insertStudentCommand.Parameters.AddWithValue("@CourseName", courseName);

                            // Execute the insert command
                            insertStudentCommand.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine($"Student {firstName} {lastName} added successfully!");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to log the error or handle it in a way that makes sense for your application.
            }
        }















    }
}
