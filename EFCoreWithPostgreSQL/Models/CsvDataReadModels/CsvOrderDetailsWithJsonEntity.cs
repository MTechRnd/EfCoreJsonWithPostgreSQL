using EFCoreJsonApp.Models.OrderWithOrderDetail;

namespace EFCoreJsonApp.Models.CsvDataReadModels
{
    public class CsvOrderDetailsWithJsonEntity
    {
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDetailsJson { get; set; }
    }
}
