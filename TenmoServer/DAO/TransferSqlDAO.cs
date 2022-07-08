using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private readonly string connectionString;
        

        public TransferSqlDAO(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }



        public Transfer NewTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
          
                const string sql = "INSERT transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES ((SELECT transfer_type_id FROM transfer_types WHERE transfer_type_desc = 'Send'), (SELECT transfer_status_id FROM transfer_statuses WHERE transfer_status_desc = 'Approved'), (SELECT account_id FROM accounts WHERE user_id = @user_id), (SELECT account_id FROM accounts WHERE user_id = @user_to_id), @amount); UPDATE accounts SET balance = ((SELECT a.balance FROM accounts a WHERE user_id = @user_id) - @amount ) WHERE user_id = @user_id; UPDATE accounts SET balance = ((SELECT a.balance FROM accounts a WHERE user_id = @user_to_id) + @amount ) WHERE user_id = @user_to_id;";
                
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@user_id", transfer.user_id );
                command.Parameters.AddWithValue("@user_to_id", transfer.user_to_id);  // were passing in the user to id in the account to slot
                command.Parameters.AddWithValue("@amount", transfer.amount);
                //TODO add protection vs non user #id
                int id = Convert.ToInt32(command.ExecuteScalar());
                transfer.transfer_id = id;
                
                return transfer;
            }
        }
        public List<Transfer> GetUsersTransfers(int user_id)
        {
            const string sql = "SELECT * FROM transfers INNER JOIN accounts ON transfers.account_from = accounts.account_id WHERE user_id = @user_id UNION SELECT * FROM transfers INNER JOIN accounts ON transfers.account_to = accounts.account_id WHERE user_id = @user_id";

            Transfer allTransfers = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@user_id", user_id);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                decimal balance = Convert.ToDecimal(reader["balance"]); // ["column name"]
                
            }

            return allTransfers;
        }
    }
}
