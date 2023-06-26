using EFCoreJsonApp.Comman;
using EFCoreJsonApp.Models.Order;

namespace EFCoreJsonApp.Models.OrderDetails
{
    public class OrderDetailEntity : BaseEntity
    {
        public Guid OrderId { get; set; }
        public string ItemName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
        public OrderEntity Order { get; set; }
    }
}
