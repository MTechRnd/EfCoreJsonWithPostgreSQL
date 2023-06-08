using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.Order;
using EFCoreJsonApp.Models.OrderDetails;
using EFCoreJsonApp.Models.Orders;
using EFCoreJsonApp.Models.OrderWithOrderDetail;
using EFCoreJsonApp.Models.OrderWithOrderDetailJson;
using EFCoreJsonApp.Services;
using EFCoreJsonApp.Services.JsonUsingLinqService;
using EFCoreJsonApp.Services.TraditionalService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EFCoreWithPostgreSQL
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<DataContext>();
                services.AddDbContext<JsonDataContext>();
                services.AddScoped<ITraditionalService, TraditionalService>();
                services.AddScoped<IJsonUsingLinqService, JsonUsingLinqService>();
            });

            using var host = hostBuilder.Build();
            using var serviceScope = host.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            // Use services
            var traditionalService = serviceProvider.GetService<ITraditionalService>();
            var id = new Guid("003caf85-8c76-4fb6-9140-18bc3ed523aa");
            var customerIds = new List<Guid>
            {
                new Guid("003caf85-8c76-4fb6-9140-18bc3ed523aa"),
                new Guid("b6fd1215-41f5-ed11-9f05-f46b8c8f0ef6"),
                new Guid("01fa1215-41f5-ed11-9f05-f46b8c8f0ef6")
            };
            Console.WriteLine("Traditional query:");
            var res1 = await traditionalService.GetAllDataAsync();
            var res2 = await traditionalService.GetDataForSingleCustomerAsync(id);
            var res3 = await traditionalService.GetDataForMultipleCustomerAsync(customerIds);
            var res4 = await traditionalService.TotalOrdersOfCustomerAsync(id);
            var res5 = await traditionalService.TotalOrdersOfCustomersAsync();
            var res6 = await traditionalService.AverageOfPriceAsync();
            var res7 = await traditionalService.AverageOfQuantityAsync();
            var res8 = await traditionalService.SumOfAllPriceAsync();
            var res9 = await traditionalService.SumOfAllQuantityAsync();
            var res10 = await traditionalService.GetMaxQuantityByOrderIdAsync(id);
            var res11 = await traditionalService.GetMinQuantityByOrderIdAsync(id);
            var res12 = await traditionalService.GetTotalByOrderIdAsync(id);
            var res13 = await traditionalService.GetMaxPriceByOrderIdAsync(id);
            var res14 = await traditionalService.GetMinPriceByOrderIdAsync(id);

            var order = new OrderEntity
            {
                CustomerName = "Smitesh",
                OrderDate = DateTime.Now,
                OrderDetails = new List<OrderDetailEntity>
                {
                    new OrderDetailEntity()
                    {
                        ItemName = "Black Pen",
                        Price = 250.00f,
                        Quantity = 3
                    },
                    new OrderDetailEntity()
                    {
                        ItemName = "Pencil Box",
                        Price = 250.00f,
                        Quantity = 3
                    }
                }
            };
            await traditionalService.InsertOrderDetailsAsync(order);


            var OrderDetailsDto = new List<OrderDetailUpdateDto>
                {
                    new OrderDetailUpdateDto(new Guid("a3e2008d-11ba-49f3-bb1c-a7b2bdadd854"),500.00f,3)
                };
            var updateOrderDetailsTraditional = new OrderUpdateDto(new Guid("000b3345-761f-44fe-bec7-10d4077e5bcb"), "smitesh maniya", OrderDetailsDto);
            var res15 = await traditionalService.UpdateOrderDetailsAsync(updateOrderDetailsTraditional);

            Console.WriteLine("Json Linq query:");
            var jsonLinqService = serviceProvider.GetService<IJsonUsingLinqService>();
            var idJson = new Guid("002b1a62-72a1-483c-8b3b-40f7c8283bdf");
            var customerIdsJson = new List<Guid>
            {
                new Guid("002b1a62-72a1-483c-8b3b-40f7c8283bdf"),
                new Guid("977827d2-19fa-ed11-9f08-f46b8c8f0ef6"),
                new Guid("708b27d2-19fa-ed11-9f08-f46b8c8f0ef6")
            };
            var resJsonLinq1 = await jsonLinqService.GetAllDataAsync();
            var resJsonLinq2 = await jsonLinqService.GetDataForSingleCustomerAsync(idJson);
            var resJsonLinq3 = await jsonLinqService.GetDataForMultipleCustomerAsync(customerIdsJson);
            var resJsonLinq4 = await jsonLinqService.TotalOrdersOfCustomerAsync(idJson);
            var resJsonLinq5 = await jsonLinqService.TotalOrdersOfCustomersAsync();
            var resJsonLinq6 = await jsonLinqService.AverageOfPriceAsync();
            var resJsonLinq7 = await jsonLinqService.AverageOfQuantityAsync();
            var resJsonLinq8 = await jsonLinqService.SumOfAllPriceAsync();
            var resJsonLinq9 = await jsonLinqService.SumOfAllQuantityAsync();
            var resJsonLinq10 = await jsonLinqService.GetMaxQuantityByOrderIdAsync(idJson);
            var resJsonLinq11 = await jsonLinqService.GetMinQuantityByOrderIdAsync(idJson);
            var resJsonLinq12 = await jsonLinqService.GetTotalByOrderIdAsync(idJson);
            var resJsonLinq13 = await jsonLinqService.GetMaxPriceByOrderIdAsync(idJson);
            var resJsonLinq14 = await jsonLinqService.GetMinPriceByOrderIdAsync(idJson);
            var orderWithOrderDetails = new OrderWithOrderDetailEntity()
            {
                CustomerName = "Smitesh",
                OrderDate = DateTime.Now,
                OrderDetailsJson = new List<OrderDetailsJson>
                {
                    new OrderDetailsJson()
                    {
                        ItemName = "Black Pen",
                        Price = 250.00f,
                        Quantity = 3,
                        Total = 750.00f
                    },
                    new OrderDetailsJson()
                    {
                        ItemName = "Pencil Box",
                        Price = 250.00f,
                        Quantity = 3,
                        Total = 750.00f
                    }
                }
            };

            await jsonLinqService.InsertOrderDetailsAsync(orderWithOrderDetails);

            var OrderDetailsJson = new List<OrderDetailsJsonDto>()
                {
                    new OrderDetailsJsonDto(0, 300, 2),
                    new OrderDetailsJsonDto(1, 400, 2)
                };

            var updateDetailJson = new OrderWithOrderDetailJsonUpdateDto(new Guid("a3cade7c-71ff-ed11-9f09-f46b8c8f0ef6"), "smitesh maniya", OrderDetailsJson);
            var resJsonLinq15 = await jsonLinqService.UpdateOrderDetailsAsync(updateDetailJson);
        }
    }
}