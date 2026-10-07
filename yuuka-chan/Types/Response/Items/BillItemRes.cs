using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan.Types.Response.Items
{
    public class BillItemRes
    {
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public float? SinglePrice { get; set; }
        public float? TotalPrice { get; set; }
    }
}
