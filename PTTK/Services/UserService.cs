using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PTTK.Models;
using PTTK.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public class UserService : IUserService
    {
        private readonly PTTKContext _context;
        private readonly IConfiguration _configuration;

        public UserService(PTTKContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            if (request == null)
                return null;

            if (request.Login == null || request.Password == null)
                return null;

            User user = _context.Users.FirstOrDefault(u => u.Login == request.Login);
            if (user == null) //login not found!
                return null;

            
            if (!PasswordValidator.isValid(request.Password, user.Password))
                return null;

            string token = generateJwtToken(user);

            return new AuthenticateResponse() { Token = token };
        }

        public User GetById(int id)
        {
            return _context.Users.Include(u => u.TuristData).ThenInclude(td => td.LeaderData).FirstOrDefault(u => u.Id == id);
        }

        private string generateJwtToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string secret = _configuration.GetSection(Constants.AppSettingsSection)[Constants.SecretValueName];
            byte[] key = Encoding.ASCII.GetBytes(secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(Constants.ClaimTypeId, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
