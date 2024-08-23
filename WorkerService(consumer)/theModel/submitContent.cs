using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_consumer_.theModel
{
    public class submitContent
    {
        [Key]
        public Guid Clordid { get; set; }
        public string Username { get; set; }
        public int Qty { get; set; }
        public decimal Px { get; set; }
        public string Dir { get; set; }
    }
}
