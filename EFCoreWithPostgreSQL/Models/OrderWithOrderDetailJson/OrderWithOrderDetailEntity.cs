using EFCoreJsonApp.Comman;

namespace EFCoreJsonApp.Models.OrderWithOrderDetail
{
    public class OrderWithOrderDetailEntity: BaseEntity
    {
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public IList<OrderDetailsJson> OrderDetailsJson { get; set; }
    }
    public class OrderDetailsJson
    {
        public string ItemName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
