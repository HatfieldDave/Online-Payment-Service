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
                command.Parameters.AddWithValue("@user_id", transfer.user_id);
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
            const string sql = "SELECT (SELECT username FROM users WHERE user_id = @user_id) AS username, t.transfer_id, ut.username AS transfer_to_name, uf.username AS transfer_from_name, t.amount, tt.transfer_type_desc, ts.transfer_status_desc FROM transfers t INNER JOIN accounts af ON t.account_from = af.account_id INNER JOIN users uf ON uf.user_id = af.user_id INNER JOIN accounts ato ON ato.account_id = t.account_to INNER JOIN users ut ON ut.user_id = ato.user_id INNER JOIN transfer_types tt ON t.transfer_type_id = tt.transfer_type_id INNER JOIN transfer_statuses ts ON t.transfer_status_id = ts.transfer_status_id WHERE uf.user_id = @user_id OR ut.user_id = @user_id";
;

            List<Transfer> allTransfers = new List<Transfer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@user_id", user_id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Transfer transfer = new Transfer();

                    string username = Convert.ToString(reader["username"]);
                    transfer.username = username;

                    int transferId = Convert.ToInt32(reader["transfer_id"]);
                    transfer.transfer_id = transferId;

                    string fromUser = Convert.ToString(reader["transfer_from_name"]);
                    transfer.transfer_from_username = fromUser;

                    string toUser = Convert.ToString(reader["transfer_to_name"]);
                    transfer.transfer_to_username = toUser;

                    string transTypeName = Convert.ToString(reader["transfer_type_desc"]);
                    transfer.transfer_type_desc = transTypeName;

                    string transStatusName = Convert.ToString(reader["transfer_status_desc"]);
                    transfer.transfer_status_desc = transStatusName;

                    decimal ammount = Convert.ToDecimal(reader["amount"]);
                    transfer.amount = ammount;

                    allTransfers.Add(transfer);
                }

                return allTransfers;
            }
        }
    }
}
