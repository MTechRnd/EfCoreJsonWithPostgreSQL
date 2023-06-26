using EFCoreJsonApp.Models.Order;
using EFCoreJsonApp.Models.OrderDetails;
using EFCoreJsonApp.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreJsonApp.Services
{
    public interface ITraditionalService
    {
        Task<IList<OrderEntity>> GetAllDataAsync();
        Task<OrderEntity> GetDataForSingleCustomerAsync(Guid id);
        Task<IList<OrderEntity>> GetDataForMultipleCustomerAsync(IList<Guid> id);
        Task<float> AverageOfPriceAsync();
        Task<double> AverageOfQuantityAsync();
        Task<int> SumOfAllQuantityAsync();
        Task<double> SumOfAllPriceAsync();
        Task<int> TotalOrdersOfCustomerAsync(Guid id);
        Task<IList<OrderCount>> TotalOrdersOfCustomersAsync();
        Task<int> GetMaxQuantityByOrderIdAsync(Guid id);
        Task<int> GetMinQuantityByOrderIdAsync(Guid id);
        Task<float> GetTotalByOrderIdAsync(Guid id);
        Task<float> GetMaxPriceByOrderIdAsync(Guid id);
        Task<float> GetMinPriceByOrderIdAsync(Guid id);
        Task InsertOrderDetailsAsync(OrderEntity order);
        Task<bool> UpdateOrderDetailsAsync(OrderUpdateDto orderDetails);
    }
}
