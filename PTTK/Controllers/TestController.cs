using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTTK.Models;
using PTTK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly PTTKContext _context;

        public TestController(PTTKContext context)
        {
            _context = context;
        }

        [Authorize]
        [Route("logins")]
        public IEnumerable<string> GetLogins()
        {
            return _context.Users.Select(u => u.Login).ToArray();
        }

        [Route("turists")]
        public IEnumerable<bool> Tourists()
        {
            return _context.Users.Select(u => u.isTurist).ToArray();
        }

        [Route("hash/{password}")]
        public string GetHash(string password)
        {
            return PasswordValidator.createHash(password);
        }

        [Route("validate/{login}/{password}")]
        public bool GetHash(string login, string password)
        {
            User user = _context.Users.First(u => u.Login == login);

            return PasswordValidator.isValid(password, user.Password);
        }
    }
}
