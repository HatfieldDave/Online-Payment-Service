using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    public class AccountBalance
    {
        [Required]
        [Range(0,double.MaxValue)]
        public decimal Balance { get; set; }
    }
}
