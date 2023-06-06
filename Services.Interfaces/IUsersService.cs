using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUsersService : ICrudService<User>
    {
        Task<User?> FindByNameAsync(string username);
        Task<User?> LoginAsync(string username, string password);
    }
}
