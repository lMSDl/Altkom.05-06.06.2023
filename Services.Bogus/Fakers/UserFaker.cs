using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakers
{
    public class UserFaker : BaseFaker<User>
    {
        public UserFaker() {

            RuleFor(x => x.Username, x => x.Internet.UserName());
            RuleFor(x => x.Password, x => BCrypt.Net.BCrypt.HashPassword(x.Internet.Password(8)));
            RuleFor(x => x.Email, x => x.Person.Email);
            RuleFor(x => x.Roles, x => (Roles)0);
        }
    }
}
