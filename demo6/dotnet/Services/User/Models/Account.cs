using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public string Role { get; set; }

        public static List<Account> GetAll()
        {
            return new List<Account>()
            {
                new Account()
                {
                    Id = 87654,
                    Name = "leo",
                    Password = "123456",
                    Role = "admin"
                },
                new Account()
                {
                    Id = 45678,
                    Name = "mickey",
                    Password = "123456",
                    Role = "normal"
                }
            };
        }
    }
}
