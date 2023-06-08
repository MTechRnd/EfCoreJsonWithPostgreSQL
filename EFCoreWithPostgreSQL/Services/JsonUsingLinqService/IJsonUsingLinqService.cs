using EFCoreJsonApp.Models.Order;
using EFCoreJsonApp.Models.OrderDetails;
using EFCoreJsonApp.Models.OrderWithOrderDetail;
using EFCoreJsonApp.Models.OrderWithOrderDetailJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreJsonApp.Services.JsonUsingLinqService
{
    public interface IJsonUsingLinqService
    {
        Task<IList<OrderWithOrderDetailEntity>> GetAllDataAsync();
        Task<OrderWithOrderDetailEntity> GetDataForSingleCustomerAsync(Guid id);
        Task<IList<OrderWithOrderDetailEntity>> GetDataForMultipleCustomerAsync(IList<Guid> ids);
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
        Task InsertOrderDetailsAsync(OrderWithOrderDetailEntity orderWithOrderDetails);
        Task<bool> UpdateOrderDetailsAsync(OrderWithOrderDetailJsonUpdateDto orderWithOrdderDetailsDto);
        Task<bool> DeleteOrdersWithOrderIdAsync(Guid orderId);
    }
}
