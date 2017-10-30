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

        public static List<Account> GetAll()
        {
            return new List<Account>()
            {
                new Account()
                {
                    Id = 1,
                    Name = "leo",
                    Password = "123456"
                },
                new Account()
                {
                    Id = 2,
                    Name = "mickey",
                    Password = "123456"
                }
            };
        }
    }
}
