﻿using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace SQL_CRM
{
    public class Program
    {
        private static string conString = @"Server = (localdb)\mssqllocaldb; DataBase = CustomerCRM; Trusted_Connection = True";
        private static void Main(string[] args)
        {
            Console.WriteLine("Categories".ToUpper());
            Console.WriteLine("1. Create new customer");
            Console.WriteLine("2. Update a customer");
            Console.WriteLine("3. Delete a customer");
            Console.WriteLine("4. Get all customers");
            Console.WriteLine("5. Quit");
            Console.WriteLine("___________________________________");

            var input = Console.ReadLine();
            if (input=="1")
                AddCustomer();
            else if (input=="2")
                UpdateCustomerInfo();
            else if (input=="3")
                DeleteCustomer();
            else if(input=="4")
                GetInfoOnCustomers();
            else
                Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("___________________________________");


        }

        public static void GetInfoOnCustomers()
        {
            var sql = @"SELECT [FirstName],[LastName] FROM CustomerInfo";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("Existing customers:".ToUpper());

                while (reader.Read())
                {
                    //var id = reader.GetString(2);
                    var firstname = reader.GetString(0);
                    var lastname = reader.GetString(1);
                   
                    
                   
                    System.Console.WriteLine($"{firstname} {lastname}");
                }

            }
            Console.WriteLine();
        }

        public static void AddCustomer()
        {
            Console.Write("Enter customers info, seperated by ' ':");
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
                Console.WriteLine("Vem vill du ändra på?");

                string input = Console.ReadLine();
                var list = getCustomersFromFirstName(input);
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

        private static List<Customer> getCustomersFromFirstName(string input)
        {
            var list = new List<Customer>();
            var sql = "Select* FROM CustomerInfo Where FirstName=@input";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                connection.Open();
                command.Parameters.Add(new SqlParameter("input", input));
                var reader=command.ExecuteReader();
                while (reader.Read())
                {
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    var phoneNumber = reader.GetString(4);
                    var id = reader.GetInt32(0);

                    list.Add(new Customer(firstName,lastName, email, phoneNumber, id));
                }

            }

            return list;
           
        }

        public static void DeleteCustomer()
        {
            Console.WriteLine("Enter the id of the customer to delete");

             var input = Console.ReadLine();

            string id = input.Split(' ')[0];
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
                        //Parameters
                        command.Parameters.Add(new SqlParameter("id", id));
                        command.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("An error occurred: {0}", ex);
            }

            Console.WriteLine($"The customer with id = {id} is deleted");
        }
    }


}

public class Customer
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int Id { get; private set; }

    public Customer()
    {
        FirstName = " ";
        LastName = " ";
        Email = " ";
        PhoneNumber = " ";

    }

    public Customer(string firstname, string lastname, string email, string phonenumber)
    {
        FirstName = firstname;
        LastName = lastname;
        Email = email;
        PhoneNumber = phonenumber;
    }
    public Customer(string firstname, string lastname, string email, string phonenumber, int id)
    {
        FirstName = firstname;
        LastName = lastname;
        Email = email;
        PhoneNumber = phonenumber;
        Id = id;
    }
}


