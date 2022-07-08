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
using System.Data.SqlClient;

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

            int id = LoggedInUserId;
            transfer.user_id = id;
            AccountBalance accountBalance = accountBalanceDAO.GetBalance(id);

            if (transfer.amount < accountBalance.Balance)
            {
                try
                {
                    Transfer newTransfer = transferDAO.NewTransfer(transfer);
                    return Ok(newTransfer);
                }
                catch (SqlException ex)
                {
                    return BadRequest(new { message = "Illegal User Selected.: " + ex.Message});
                }


            }
            else if (transfer.amount <= 0)
            {
                return BadRequest(new { message = "Transfer must be more than 0 and have enough funds" }); //TODO remove full error message
            }
            return BadRequest(new { message = "Can only choose accounts that exist" });

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
