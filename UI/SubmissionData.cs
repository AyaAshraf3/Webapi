using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    public class SubmissionData
    {
            public Guid Clordid { get; set; }
            public string Username { get; set; }
            public int Qty { get; set; }
            public decimal Px { get; set; }
            public string Dir { get; set; }

    }
}
