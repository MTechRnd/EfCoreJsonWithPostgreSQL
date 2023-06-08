using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreJsonApp.Models.OrderDetails
{
    public class OrderCount
    {
        public Guid Id { get; set; }
        public int TotalOrder { get; set; }
    }
}
