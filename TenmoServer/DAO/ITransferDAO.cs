using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer NewTransfer(Transfer transfer);

        List<Transfer> GetUsersTransfers(int user_id);
    }
}
