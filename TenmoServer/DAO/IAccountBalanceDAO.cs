using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountBalanceDAO
    {
        AccountBalance GetBalance(int user_id);
    }
}