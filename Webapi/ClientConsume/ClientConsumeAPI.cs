using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Webapi.ClientConsume
{
    public class ClientConsumeAPI
    {
        [Key]
        public Guid Clordid { get; set; } 
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal Px { get; set; }
        public string Dir { get; set; }
    }
}

