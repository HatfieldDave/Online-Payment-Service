using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int user_id { get; set; } 
        public string username { get; set; }
        public int transfer_id { get; set; }
        public int transfer_type_id { get; set; }
        public int account_from { get; set; }
        public int account_to { get; set; }
        public decimal amount { get; set; }
      
    }
}
