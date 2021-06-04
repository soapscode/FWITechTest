using System;
using TechTestLib.Interfaces;

namespace Interview
{
    public class Customer : ICustomer
    {
        public IComparable Id { get; set; }
        public string FirstName { get; set;  }
        public string Surname { get; set; }
    }
}