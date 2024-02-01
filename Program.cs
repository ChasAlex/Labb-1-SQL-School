using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace Labb_1_SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            string connectionString = "Data Source=(localdb)\\.;Initial Catalog=SQL-Lab;Integrated Security=True";
            bool is_running = true;
            string menu = " 1.Fetch all students \n 2.Fetch all students in a class \n 3.Add Staff \n 4.Fetch Staff \n 5.Fetch all grades last month \n 6.Average grade course \n 7. Add Student \n 8. Exit \n \n Enter command: ";
            DataHandler handler = new DataHandler();
            while (is_running)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the student database");
                Console.Write(menu);

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string input = Console.ReadLine();
                        

                        switch (input)
                        {
                            case "1":

                                handler.Fetch_all_Students(connection);
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;
                            case "2":

                                handler.Fetch_Students_class(connection);
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;

                            case "3":
                                handler.Add_Teacher(connection);
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;

                            case "4":
                                handler.Fetch_Teacher(connection);
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;

                            case "5":
                                handler.FetchGrades(connection);    
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;
                            case "6":
                                handler.FetchCourseStatistics(connection);
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;
                            case "7":
                                handler.AddStudent(connection); 
                                Console.WriteLine("Press Enter to return to menu...");
                                Console.ReadLine();
                                break;
                            case "8":
                                Console.WriteLine("Exiting system...");
                                is_running = false;
                                break;
                            default:
                                // Handle invalid input
                                Console.WriteLine("Invalid input. Please enter a number between 1 and 8.");
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ReadLine();
                }
            }




        }
    }
}