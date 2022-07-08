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
        private readonly ITransferDAO transferDAO;

        public Account(IAccountBalanceDAO accountBalanceDAO, IUserDAO userDAO, ITransferDAO transferDAO)
        {
            this.accountBalanceDAO = accountBalanceDAO;
            this.userDAO = userDAO;
            this.transferDAO = transferDAO;
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

        [HttpPost("transfers")]
        public ActionResult PostNewTransfer(Transfer transfer)
        {
            // TODO: Let's explore User.Identity.Name
            //string username = User.Identity.Name; // Built into ASP .NET, gets the name from JWT
                                                  // TODO: Associate the joke with the user's logged in name
            int id = LoggedInUserId; // A custom derived property, defined below
            transfer.user_id = id;                         // This ID came from the JWT's "sub" claim. sub == subject or the ID of the user.
            AccountBalance accountBalance = accountBalanceDAO.GetBalance(id);
            // transfer.transfer_id = id;
            if (transfer.amount < accountBalance.Balance)
            {
            Transfer newTransfer = transferDAO.NewTransfer(transfer);

            return Ok(newTransfer); // or Created("jokes/" + createdJoke.Id, createdJoke);

            }
            else
            {
                return BadRequest("Insuffcient funds for transfer.");
            }

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
