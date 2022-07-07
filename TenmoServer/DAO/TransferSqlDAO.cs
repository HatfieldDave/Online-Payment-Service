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

        //public List<User> GetAllOtherUsers(int user_id)
        //{
        //    List<User> otherUsers = new List<User>();
        //    //TranferSqlDAO transferDao = new TranferSqlDAO();
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand command = new SqlCommand(GetAllOtherUsersSQL, conn);
        //        command.Parameters.AddWithValue("@user", user_id);

        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {

        //            User user = new User();
        //            user.Username = Convert.ToString(reader["username"]); // ["column name"]
        //            otherUsers.Add(user);
        //        }
        //    }
        //    return otherUsers;
        //}
    }






}
