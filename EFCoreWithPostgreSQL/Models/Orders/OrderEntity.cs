using EFCoreJsonApp.Comman;
using EFCoreJsonApp.Models.OrderDetails;

namespace EFCoreJsonApp.Models.Order
{
    public class OrderEntity : BaseEntity
    {
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public IList<OrderDetailEntity> OrderDetails { get; set; }
    }

}
