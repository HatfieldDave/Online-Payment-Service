using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly string SQLBalanceString = "select balance from users inner join accounts on accounts.user_id = users.user_id where users.username = @name;";


        public Account()
        {
            // to do dao stuff and things 
        }

        [HttpGet("")] // may need to change inside ""
        public AccountBalance GetBalance(string username)
        {
            //AccountBalance accountBalance = new
            return null;
        }

    }
}
