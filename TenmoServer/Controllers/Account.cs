using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using TenmoServer.DAO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class Account : ControllerBase
    {
       
        private readonly IAccountBalanceDAO accountBalanceDAO;
        private readonly IUserDAO userDAO;

        public Account(IAccountBalanceDAO accountBalanceDAO, IUserDAO userDAO)
        {
            this.accountBalanceDAO = accountBalanceDAO;
            this.userDAO = userDAO;
            // to do dao stuff and things 
        }

        [HttpGet("balance")] 
        public ActionResult GetBalance()
        {
            int id = LoggedInUserId;
            AccountBalance accountBalance = accountBalanceDAO.GetBalance(id);
            return Ok(accountBalance);
        }

        [HttpGet("users")]  //TODO switch to username if doesn't work
        public ActionResult GetOtherUsers()
        {
            int id = LoggedInUserId;
            List<User> otherUsers = userDAO.GetAllOtherUsers(id);
                return Ok(otherUsers);
        }

        private int LoggedInUserId
        {
            get
            {
                Claim idClaim = User.FindFirst("sub");
                if (idClaim == null)
                {
                    // User is not logged in
                    return -1;
                }
                else
                {
                    // User is logged in. Their subject (sub) claim is their ID
                    return int.Parse(idClaim.Value);
                }
            }
        }

    }
}
