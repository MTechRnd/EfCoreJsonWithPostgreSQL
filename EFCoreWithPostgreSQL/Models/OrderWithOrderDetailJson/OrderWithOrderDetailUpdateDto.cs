namespace EFCoreJsonApp.Models.OrderWithOrderDetailJson
{
    public record OrderWithOrderDetailJsonUpdateDto(Guid Id, string CustomerName, IList<OrderDetailsJsonDto> OrderDetails);
    public record OrderDetailsJsonDto(int ListIndex, float Price, int Quantity);
}
