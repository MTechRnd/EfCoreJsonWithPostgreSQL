using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.OrderDetails;
using EFCoreJsonApp.Models.OrderWithOrderDetail;
using EFCoreJsonApp.Models.OrderWithOrderDetailJson;
using EFCoreJsonApp.Models.Records;
using Microsoft.EntityFrameworkCore;

namespace EFCoreJsonApp.Services.JsonUsingLinqService
{
    public class JsonUsingLinqService : IJsonUsingLinqService
    {
        private JsonDataContext _context;

        public JsonUsingLinqService(JsonDataContext context)
        {
            _context = context;
        }


        public async Task InsertOrderDetailsAsync(OrderWithOrderDetailEntity orderWithOrderDetails)
        {
            _context.OrderWithOrderDetails.Add(orderWithOrderDetails);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateOrderDetailsAsync(OrderWithOrderDetailJsonUpdateDto orderWithOrdderDetailsDto)
        {
            var result = await _context.OrderWithOrderDetails.FirstOrDefaultAsync(o => o.Id == orderWithOrdderDetailsDto.Id);
            if (result == null)
                return false;
            result.CustomerName = orderWithOrdderDetailsDto.CustomerName;
            foreach (var orderDetail in orderWithOrdderDetailsDto.OrderDetails)
            {
                if (orderDetail.ListIndex >= 0 && orderDetail.ListIndex < result.OrderDetailsJson.Count())
                {
                    result.OrderDetailsJson[orderDetail.ListIndex].Price = orderDetail.Price;
                    result.OrderDetailsJson[orderDetail.ListIndex].Quantity = orderDetail.Quantity;
                    result.OrderDetailsJson[orderDetail.ListIndex].Total = orderDetail.Price * orderDetail.Quantity;
                }
            }
            _context.Entry(result).Property(e => e.OrderDetailsJson).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrdersWithOrderIdAsync(Guid orderId)
        {
            var getOrder = await _context.OrderWithOrderDetails.FindAsync(orderId);
            if (getOrder != null)
            {
                _context.OrderWithOrderDetails.Remove(getOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<AverageOfPriceResult> AverageOfPriceAsync()
        {
            var query = @"
                    SELECT
                    AVG(CAST(json_data ->> 'Price' AS real)) AS AverageOfPrice
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data";
            var result = await _context.Set<AverageOfPriceResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<AverageOfQuantityResult> AverageOfQuantityAsync()
        {
            var query = @"                    
                    SELECT
                    AVG(CAST(json_data ->> 'Quantity' AS real)) AS AverageOfQuantity
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data";
            var result = await _context.Set<AverageOfQuantityResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<TotalPriceResult> SumOfAllPriceAsync()
        {
            var query = @"
                    SELECT
                    SUM(CAST(json_data ->> 'Price' AS real)) AS TotalPrice
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data";
            var result = await _context.Set<TotalPriceResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<TotalQuantityResult> SumOfAllQuantityAsync()
        {
            var query = @"
                    SELECT
                    SUM(CAST(json_data ->> 'Quantity' AS Integer)) AS TotalQuantity
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data";
            var result = await _context.Set<TotalQuantityResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<IList<OrderWithOrderDetailEntity>> GetAllDataAsync()
        {
            var result = await _context.OrderWithOrderDetails.ToListAsync();
            return result;
        }

        public async Task<IList<OrderWithOrderDetailEntity>> GetDataForMultipleCustomerAsync(IList<Guid> customerIds)
        {
            var result = await _context.OrderWithOrderDetails
                .Where(od => customerIds.Contains(od.Id))
                .ToListAsync();
            return result;
        }

        public async Task<OrderWithOrderDetailEntity> GetDataForSingleCustomerAsync(Guid id)
        {
            var result = await _context.OrderWithOrderDetails
                .FirstOrDefaultAsync(od => od.Id == id);
            return result;
        }

        public async Task<TotalOrderByCustomerResult> TotalOrdersOfCustomerAsync(Guid id)
        {
            var query = @$"
                    SELECT jsonb_array_length(""OrderDetailsJson"") AS TotalOrderByCustomerId
                    FROM ""OrderWithOrderDetails""
                    where ""OrderWithOrderDetails"".""Id"" = '{id}'
            ";
            var result = await _context.Set<TotalOrderByCustomerResult>()
                .FromSqlRaw(query)
                .ToListAsync();

            return result[0];
        }

        public async Task<IList<OrderCount>> TotalOrdersOfCustomersAsync()
        {
            var query = @"
                SELECT ""Id"", jsonb_array_length(""OrderDetailsJson"") AS TotalOrder
                FROM ""OrderWithOrderDetails""
            ";
            var result = await _context.Set<OrderCount>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result;
        }

        public async Task<MaxQuantityResult> GetMaxQuantityByOrderIdAsync(Guid id)
        {
            var query = @$"
                SELECT
                MAX(CAST(json_data ->> 'Quantity' AS Integer)) AS MaximumQuantity
                FROM
                ""OrderWithOrderDetails"",
                jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data
                WHERE ""OrderWithOrderDetails"".""Id"" = '{id}'
                ";
            var result = await _context.Set<MaxQuantityResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<MinQuantityResult> GetMinQuantityByOrderIdAsync(Guid id)
        {
            var query = @$"
                SELECT
                MIN(CAST(json_data ->> 'Quantity' AS Integer)) AS MinimumQuantity
                FROM
                ""OrderWithOrderDetails"",
                jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data
                WHERE ""OrderWithOrderDetails"".""Id"" = '{id}'
                ";
            var result = await _context.Set<MinQuantityResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<TotalByOrderResult> GetTotalByOrderIdAsync(Guid id)
        {
            var query = @$"
                    SELECT
                    SUM(CAST(json_data ->> 'Total' AS real)) AS TotalByOrderId
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data
                    WHERE ""OrderWithOrderDetails"".""Id"" = '{id}'
                ";
            var result = await _context.Set<TotalByOrderResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<MaxPriceResult> GetMaxPriceByOrderIdAsync(Guid id)
        {
            var query = @$"
                    SELECT
                    MAX(CAST(json_data ->> 'Price' AS real)) AS MaximumPrice
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data
                    WHERE ""OrderWithOrderDetails"".""Id"" = '{id}'
                ";
            var result = await _context
                .Set<MaxPriceResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }

        public async Task<MinPriceResult> GetMinPriceByOrderIdAsync(Guid id)
        {
            var query = @$"
                    SELECT
                    MIN(CAST(json_data ->> 'Price' AS real)) AS MinimumPrice
                    FROM
                    ""OrderWithOrderDetails"",
                    jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data
                    WHERE ""OrderWithOrderDetails"".""Id"" = '{id}'
                ";
            var result = await _context.Set<MinPriceResult>()
                .FromSqlRaw(query)
                .ToListAsync();
            return result[0];
        }
    }
}
