using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions.API.Models;

namespace Transactions.API.Interfaces
{
    public interface ILoginService
    {
        User AuthenticateUser(User loginCredentials);
        string GenerateJWT(User userInfo);
    }
}
