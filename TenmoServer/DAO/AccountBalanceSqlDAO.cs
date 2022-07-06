using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class AccountBalanceSqlDAO : IAccountBalanceDAO
    {
        private readonly string connectionString;
        public AccountBalanceSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public AccountBalance GetBalance(int user_id)
        {
            AccountBalance newBalance = new AccountBalance();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT balance FROM users u INNER JOIN accounts on accounts.user_id = u.user_id WHERE u.user_id = @id", conn);
                command.Parameters.AddWithValue("@id", user_id);
                
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                decimal balance = Convert.ToDecimal(reader["balance"]); // ["column name"]
                newBalance.Balance = balance;
            }

            return newBalance;
        }

    }

}
