using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class UsersService : CrudService<User>, IUsersService
    {
        public UsersService(BaseFaker<User> faker) : base(faker)
        {
        }

        public Task<User?> FindByNameAsync(string username)
        {
            return Task.FromResult(Entities.FirstOrDefault(x => x.Username.Contains(username)));
        }

        public Task<User?> LoginAsync(string username, string password)
        {
            return Task.FromResult(Entities.Where(x => x.Username == username).SingleOrDefault(x => BCrypt.Net.BCrypt.Verify(password, x.Password)));
        }

        public override Task<User> CreateAsync(User entity)
        {
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            return base.CreateAsync(entity);
        }
    }
}
