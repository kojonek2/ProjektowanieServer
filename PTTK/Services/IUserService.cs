using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public interface IUserService
    {
        public AuthenticateResponse Authenticate(AuthenticateRequest request);

        public User GetById(int id);
    }
}
