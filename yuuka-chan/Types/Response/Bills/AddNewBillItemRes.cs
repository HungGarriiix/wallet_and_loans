using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yuuka_chan.Types.Response.Items;

namespace yuuka_chan.Types.Response.Bills
{
    public class AddNewBillItemRes
    {
        public BillItemRes Item { get; set; }
        public double ExpectedBalance { get; set; }
        public List<BillItemRes> BillItems { get; set; }
    }
}
