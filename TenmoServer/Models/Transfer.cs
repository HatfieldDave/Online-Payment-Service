using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int user_id { get; set; }
        
        [StringLength(50)]
        public string username { get; set; }
        public int transfer_id { get; set; }
        public int transfer_type_id { get; set; }
        public int account_from { get; set; }
        
        [Required]
        public int account_to { get; set; }
        
        [Required]
        [Range(0,double.MaxValue)]
        public decimal amount { get; set; }
        public int user_to_id { get; set; }

    }
}
