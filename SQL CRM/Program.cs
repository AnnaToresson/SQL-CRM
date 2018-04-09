using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace SQL_CRM
{
    public class Program
    {
        private static string conString = @"Server = (localdb)\mssqllocaldb; DataBase = CustomerCRM; Trusted_Connection = True";
        private static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {
            while (true)
            {
                DisplayCategories();

                var input = Console.ReadLine();

                if (input == "1")
                    AddCustomer();
                else if (input == "2")
                    UpdateCustomerInfo();
                else if (input == "3")
                    DeleteCustomer();
                else if (input == "4")
                {
                    var list = GetInfoOnCustomers();
                    foreach (var item in list)
                    {
                        PrintCustomer(item);
                    }

                }
                else if (input == "5")
                {
                    Console.Clear();

                }
                else if (input == "6")
                    break;


                Console.WriteLine();
                Console.WriteLine("___________________________________");
            }

        }

        private static void DisplayCategories()
        {
            Console.WriteLine("Categories".ToUpper());
            Console.WriteLine("1. Create new customer");
            Console.WriteLine("2. Update a customer");
            Console.WriteLine("3. Delete a customer");
            Console.WriteLine("4. Get all customers");
            Console.WriteLine("5. Clear screen");
            Console.WriteLine("6. Quit");

            Console.WriteLine("___________________________________");
        }

        public static List<Customer> GetInfoOnCustomers()
        {
            var list = new List<Customer>();
            var sql = @"SELECT * FROM CustomerInfo";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("Existing customers:".ToUpper());

                while (reader.Read())
                {

                    var firstname = reader.GetString(1);
                    var lastname = reader.GetString(2);
                    var email = reader.GetString(3);
                    var phoneNumber = reader.GetString(4);
                    var id = reader.GetInt32(0);
                    list.Add(new Customer(firstname, lastname, email, phoneNumber, id));

                    //System.Console.WriteLine($"{firstname} {lastname}");
                }

            }

            return list;
        }

        public static void AddCustomer()
        {
            Console.Write("Enter a new customers info, seperated by ' ':");
            try
            {
                var input = Console.ReadLine();

                string firstName = input.Split(' ')[0];
                string lastName = input.Split(' ')[1];
                string email = input.Split(' ')[2];
                string phoneNumber = input.Split(' ')[3];

                string sql = $"INSERT INTO CustomerInfo (FirstName, LastName, Email, PhoneNumber) VALUES ('{firstName}', '{lastName}', '{email}', '{phoneNumber}')";


                using (SqlConnection connection = new SqlConnection(conString))
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    connection.Open();
                    command.ExecuteNonQuery();

                }

                Console.WriteLine($"{firstName} {lastName} is added to the database");
            }

            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: {0}", ex);
            }

        }

        public static void PrintCustomer(Customer customer)
        {
            Console.WriteLine($"{customer.FirstName} {customer.LastName}, {customer.Email}, {customer.PhoneNumber}");
        }

        public static void UpdateCustomerInfo()
        {

            try
            {
                Console.WriteLine("Enter a customer to update");

                string input = Console.ReadLine();
                var list = GetCustomersFromFirstName(input);
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"{i + 1} {list[i].FirstName} {list[i].LastName}");
                }

                input = Console.ReadLine();
                int index = int.Parse(input) - 1;
                PrintCustomer(list[index]);

                Console.Write("Enter customers firstname, lastname, email, phonenumber and Id-number, seperated by ' ':");
                input = Console.ReadLine();

                string firstName = input.Split(' ')[0];
                //string id = input.Split(' ')[4];
                string lastName = input.Split(' ')[1];
                string email = input.Split(' ')[2];
                string phoneNumber = input.Split(' ')[3];


                string sql = $"UPDATE CustomerInfo SET FirstName=@firstName, LastName=@lastName, Email=@email, PhoneNumber=@phonenumber WHERE Id=@id";
                using (SqlConnection connection = new SqlConnection(conString))
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("firstName", firstName));
                    command.Parameters.Add(new SqlParameter("lastName", lastName));
                    command.Parameters.Add(new SqlParameter("email", email));
                    command.Parameters.Add(new SqlParameter("phoneNumber", phoneNumber));
                    command.Parameters.Add(new SqlParameter("id", list[index].Id));
                    command.ExecuteNonQuery();

                }
                Console.WriteLine($"The customer is updated");

            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: {0}", ex);

            }


        }

        private static List<Customer> GetCustomersFromFirstName(string input)
        {
            var list = new List<Customer>();
            var sql = "Select* FROM CustomerInfo Where FirstName=@input";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                connection.Open();
                command.Parameters.Add(new SqlParameter("input", input));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    var phoneNumber = reader.GetString(4);

                    list.Add(new Customer(firstName, lastName, email, phoneNumber, id));
                }
            }

            return list;

        }

        public static void DeleteCustomer()
        {
            Console.WriteLine("Enter a customer to delete");

            string input = Console.ReadLine();
            var list = GetCustomersFromFirstName(input);
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1} {list[i].FirstName} {list[i].LastName}");


            }
            input = Console.ReadLine();
            int index = int.Parse(input) - 1;
            PrintCustomer(list[index]);


            //string lastName = input.Split(' ')[1];
            //string email = input.Split(' ')[2];
            //string phoneNumber = input.Split(' ')[3];

            string sql = $"DELETE FROM CustomerInfo WHERE Id=@id";


            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.Parameters.Add(new SqlParameter("id", list[index].Id));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("An error occurred: {0}", ex);
            }

            Console.WriteLine($"The customer is deleted");
        }
    }
}




