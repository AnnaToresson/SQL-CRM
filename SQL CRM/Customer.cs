using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_CRM
{
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
}
