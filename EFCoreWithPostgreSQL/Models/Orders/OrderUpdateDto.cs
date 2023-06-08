using EFCoreJsonApp.Models.OrderDetails;

namespace EFCoreJsonApp.Models.Orders
{
    public record OrderUpdateDto(Guid Id, string CustomerName, IList<OrderDetailUpdateDto> OrderDetails);
}
