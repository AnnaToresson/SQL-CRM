using System.Data.SqlClient;
using System;

namespace SQL_CRM
{
    public class Program
    {
        private static string conString = @"Server = (localdb)\mssqllocaldb; DataBase = CustomerCRM; Trusted_Connection = True";
        private static void Main(string[] args)
        {
            //GetInfo();
            AddCustomer();


        }

        public static void GetInfo()
        {
            var sql = @"SELECT [FirstName],[LastName] FROM CustomerInfo";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var firstname = reader.GetString(0);
                    var lastname = reader.GetString(1);
                    //var email = reader.GetString(2);
                    //var phoneNumber = reader.GetString(3);

                    System.Console.WriteLine($"Kundens namn: {firstname} {lastname}");
                }

            }
        }

        public static void AddCustomer()
        {
            var input = Console.ReadLine();

            string firstName = input.Split(' ')[0];
            string lastName = input.Split(' ')[1];
            string email = input.Split(' ')[2];
            string phoneNumber = input.Split(' ')[3];

            string sql = $"INSERT INTO CustomerInfo (FirstName, LastName, Email, PhoneNumber) VALUES ('{firstName}', '{lastName}', '{email}', '{phoneNumber}')";

            //var customer = new Customer();

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
             
                connection.Open();
                command.ExecuteNonQuery();
               

            }

        }

        public static void UpdateCustomer()
        {

            string updateString = @"
updateCustomerInfo
set FirstName ='Lisa'
where Id=2";
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand(updateString);
            command.Connection = connection;
        }

        public static void DeletCustomer(string table, string columnName, string IDNumber)
        {


            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("DELETE FROM " + table + " WHERE " + columnName + " = '" + IDNumber + "'", connection))
                    {
                        connection.Open();

                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("An error occurred: {0}", ex.Message);
            }
        }
    }




}

public class Customer
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

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
}


