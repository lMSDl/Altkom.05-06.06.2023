using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Services
{
    public class AuthService
    {
        private readonly IUsersService _service;
        public static readonly byte[] KEY; 


        static AuthService()
        {
            var guid = Guid.NewGuid();
            Console.WriteLine(guid.ToString());
            KEY = guid.ToByteArray();
        }

        public AuthService(IUsersService service)
        {
            _service = service;
        }

        public async Task<string?> AuthAsync(string login, string password)
        {
            var user = await _service.LoginAsync(login, password);
            if (user == null)
            {
                return null;
            }


            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            /*claims.Add(new Claim(ClaimTypes.Role, "Delete"));
            claims.Add(new Claim(ClaimTypes.Role, "Create"));
            claims.Add(new Claim(ClaimTypes.Role, "Read"));
            claims.Add(new Claim(ClaimTypes.Role, "UserAdmin"));*/

            claims.AddRange(user.Roles.ToString().Split(", ").Select(x => new Claim(ClaimTypes.Role, x)));



            var tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.Expires = DateTime.Now.AddMinutes(5);
            tokenDescriptor.Subject = new ClaimsIdentity(claims);
            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KEY), SecurityAlgorithms.HmacSha256Signature);
            tokenDescriptor.Audience = "abc";
            tokenDescriptor.Issuer = "zxc";

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
