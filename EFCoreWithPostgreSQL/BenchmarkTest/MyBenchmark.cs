using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.Order;
using EFCoreJsonApp.Models.OrderDetails;
using EFCoreJsonApp.Models.Orders;
using EFCoreJsonApp.Models.OrderWithOrderDetail;
using EFCoreJsonApp.Models.OrderWithOrderDetailJson;
using EFCoreJsonApp.Services;
using EFCoreJsonApp.Services.JsonUsingLinqService;
using EFCoreJsonApp.Services.TraditionalService;

namespace EFCoreJsonApp.BenchmarkTest
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class MyBenchmark
    {
        private ITraditionalService _traditionalService;
        private IJsonUsingLinqService _jsonService;
        private IJsonUsingLinqService _jsonUsingLinqService;
        private DataContext _dataContext;
        private JsonDataContext _jsonDataContext;
        private IList<Guid> _guidsOfTraditional;
        private IList<Guid> _guidsOfJson;
        private Guid _guidTraditional;
        private Guid _guidJson;
        private OrderEntity _orderTraditional;
        private List<OrderDetailEntity> _orderDetailsTraditional;
        private OrderWithOrderDetailEntity _orderWithOrderDetailsLinq;
        private IList<float> Prices;
        private IList<int> Quantities;
        private Random RandomIndex;
        private IList<string> CustomerNames;

        [GlobalSetup]
        public async void Setup()
        {
            _dataContext = new DataContext();
            _traditionalService = new TraditionalService(_dataContext);
            _jsonDataContext = new JsonDataContext();
            _jsonUsingLinqService = new JsonUsingLinqService(_jsonDataContext);
            _guidsOfTraditional = new List<Guid>
            {
                new Guid("1ebb3cb6-e9de-4798-bffa-a0968ce9ce90"),
                new Guid("31670b09-b868-44b8-8aa8-acc21fcc4d5d"),
                new Guid("65fbf051-52c8-423d-a9a4-3a749183e025"),
                new Guid("50faa1f4-7854-425c-a380-af500b6db9f4"),
                new Guid("7a39c1ca-e50d-4f0c-ad1d-e6c273bd662d")
            };
            _guidsOfJson = new List<Guid>
            {
                new Guid("3953735a-e2e0-4e83-8773-5ded4c304fd5"),
                new Guid("3af00af9-e365-4f81-a0dd-d03598c038af"),
                new Guid("523b3033-6f6c-4d32-afe8-c2d98c269dca"),
                new Guid("6c2f59a9-a2a0-4e4d-96b9-65409109f847"),
                new Guid("7ebb28e4-bf64-43f7-9a93-595f9c6307d4"),
            };
            _guidTraditional = new Guid("1ebb3cb6-e9de-4798-bffa-a0968ce9ce90");
            _guidJson = new Guid("7ebb28e4-bf64-43f7-9a93-595f9c6307d4");

            Prices = new List<float>
            {
                550.50f, 780.34f, 235.50f, 200.00f, 499.00f, 1299.00f, 500.00f, 260.80f, 300.90f, 400.00f
            };

            Quantities = new List<int>
            {
                4, 5, 8, 9, 1, 2, 3, 10, 7, 6
            };

            CustomerNames = new List<string>
            {
                "Milan",
                "Jay",
                "Rahul",
                "Vijay",
                "Nainesh",
                "Amit",
                "Vishal",
                "Raj"
            };

            RandomIndex = new Random();
            DeleteOtherRecordsOfOrderEntity();
            DeleteOtherRecordsOfOrderWithOrderDetailsEntity();
        }

        public void DeleteOtherRecordsOfOrderEntity()
        {
            var totalRecords = _dataContext.Orders.Count();
            var removeRecors = totalRecords - 10000;
            if (removeRecors > 0)
            {
                var recordsToDelete = _dataContext.Orders.OrderByDescending(o => o.Id).Take(removeRecors).Select(o => o.Id).ToList();
                var records = _dataContext.Orders.Where(o => recordsToDelete.Contains(o.Id)).ToList();
                _dataContext.Orders.RemoveRange(records);
                _dataContext.SaveChanges();
            }
        }
        public void DeleteOtherRecordsOfOrderWithOrderDetailsEntity()
        {
            var totalRecords = _jsonDataContext.OrderWithOrderDetails.Count();
            var removeRecors = totalRecords - 10000;
            if (removeRecors > 0)
            {
                var recordsToDelete = _jsonDataContext.OrderWithOrderDetails.OrderByDescending(o => o.Id).Take(removeRecors).Select(o => o.Id).ToList();
                var records = _jsonDataContext.OrderWithOrderDetails.Where(o => recordsToDelete.Contains(o.Id)).ToList();
                _jsonDataContext.OrderWithOrderDetails.RemoveRange(records);
                _jsonDataContext.SaveChanges();
            }
        }

        [Benchmark(Baseline = true)]
        public async Task TraditionalBenchmark()
        {
            //var res1 = await _traditionalService.GetAllDataAsync();
            //var res2 = await _traditionalService.GetDataForSingleCustomerAsync(_guidTraditional);
            //var res3 = await _traditionalService.GetDataForMultipleCustomerAsync(_guidsOfTraditional);
            //var res4 = await _traditionalService.TotalOrdersOfCustomerAsync(_guidTraditional);
            //var res5 = await _traditionalService.TotalOrdersOfCustomersAsync();
            //var res6 = await _traditionalService.AverageOfPriceAsync();
            //var res7 = await _traditionalService.AverageOfQuantityAsync();
            //var res8 = await _traditionalService.SumOfAllPriceAsync();
            //var res9 = await _traditionalService.SumOfAllQuantityAsync();
            //var res10 = await _traditionalService.GetMaxQuantityByOrderIdAsync(_guidTraditional);
            //var res11 = await _traditionalService.GetMinQuantityByOrderIdAsync(_guidTraditional);
            //var res12 = await _traditionalService.GetTotalByOrderIdAsync(_guidTraditional);
            //var res13 = await _traditionalService.GetMaxPriceByOrderIdAsync(_guidTraditional);
            //var res14 = await _traditionalService.GetMinPriceByOrderIdAsync(_guidTraditional);

            //var order = new OrderEntity
            //{
            //    CustomerName = "Smitesh",
            //    OrderDate = DateTime.Now,
            //    OrderDetails = new List<OrderDetailEntity>
            //    {
            //        new OrderDetailEntity()
            //        {
            //            ItemName = "Show Piece",
            //            Price = 250.00f,
            //            Quantity = 3
            //        },
            //        new OrderDetailEntity()
            //        {
            //            ItemName = "Sho Piece of Camle",
            //            Price = 250.00f,
            //            Quantity = 3
            //        },
            //        new OrderDetailEntity()
            //        {
            //            ItemName = "Show Piece",
            //            Price = 250.00f,
            //            Quantity = 3
            //        },
            //        new OrderDetailEntity()
            //        {
            //            ItemName = "Sho Piece of Camle",
            //            Price = 250.00f,
            //            Quantity = 3
            //        },
            //        new OrderDetailEntity()
            //        {
            //            ItemName = "Show Piece",
            //            Price = 250.00f,
            //            Quantity = 3
            //        }
            //    }
            //};
            //await _traditionalService.InsertOrderDetailsAsync(order);

            int PriceRandomizer = RandomIndex.Next(0, Prices.Count);
            int QuantityRandomizer = RandomIndex.Next(0, Prices.Count);
            int NameRandomizer = RandomIndex.Next(0, CustomerNames.Count);
            List<OrderDetailUpdateDto> OrderDetailsDto = new List<OrderDetailUpdateDto>
            {
                new OrderDetailUpdateDto(new Guid("3a1b2dd1-9bce-4113-893d-02137e2bb817"),Prices[PriceRandomizer],Quantities[QuantityRandomizer]),
                new OrderDetailUpdateDto(new Guid("0f3d58b4-3f84-4b3b-9004-219624e1e8f1"),Prices[PriceRandomizer],Quantities[QuantityRandomizer])
            };
            var updateOrderDetailsTraditional = new OrderUpdateDto(new Guid("65fbf051-52c8-423d-a9a4-3a749183e025"), CustomerNames[NameRandomizer], OrderDetailsDto);
            await _traditionalService.UpdateOrderDetailsAsync(updateOrderDetailsTraditional);
        }

        [Benchmark]
        public async Task JsonLinqBenchmark()
        {
            var resJson1 = await _jsonUsingLinqService.GetAllDataAsync();
            //var resJson2 = await _jsonUsingLinqService.GetDataForSingleCustomerAsync(_guidJson);
            //var resJson3 = await _jsonUsingLinqService.GetDataForMultipleCustomerAsync(_guidsOfJson);
            //var resJson4 = _jsonUsingLinqService.TotalOrdersOfCustomerAsync(_guidJson);
            //var resJson5 = _jsonUsingLinqService.TotalOrdersOfCustomersAsync();
            //var resJson6 = _jsonUsingLinqService.AverageOfPriceAsync();
            //var resJson7 = _jsonUsingLinqService.AverageOfQuantityAsync();
            //var resJson8 = _jsonUsingLinqService.SumOfAllPriceAsync();
            //var resJson9 = _jsonUsingLinqService.SumOfAllQuantityAsync();
            //var resJson10 = await _jsonUsingLinqService.GetMaxQuantityByOrderIdAsync(_guidJson);
            //var resJson11 = _jsonUsingLinqService.GetMinQuantityByOrderIdAsync(_guidJson);
            //var resJson12 = _jsonUsingLinqService.GetTotalByOrderIdAsync(_guidJson);
            //var resJson13 = _jsonUsingLinqService.GetMaxPriceByOrderIdAsync(_guidJson);
            //var resJson14 = _jsonUsingLinqService.GetMinPriceByOrderIdAsync(_guidJson);


            //var orderWithOrderDetails = new OrderWithOrderDetailEntity()
            //{
            //    CustomerName = "Smitesh",
            //    OrderDate = DateTime.Now,
            //    OrderDetailsJson = new List<OrderDetailsJson>
            //    {
            //        new OrderDetailsJson()
            //        {
            //            ItemName = "Show Piece",
            //            Price = 250.00f,
            //            Quantity = 3,
            //            Total = 750.00f
            //        },
            //        new OrderDetailsJson()
            //        {
            //            ItemName = "Sho Piece of Camle",
            //            Price = 250.00f,
            //            Quantity = 3,
            //            Total = 750.00f
            //        },
            //        new OrderDetailsJson()
            //        {
            //            ItemName = "Show Piece",
            //            Price = 250.00f,
            //            Quantity = 3,
            //            Total = 750.00f
            //        },
            //        new OrderDetailsJson()
            //        {
            //            ItemName = "Sho Piece of Camle",
            //            Price = 250.00f,
            //            Quantity = 3,
            //            Total = 750.00f
            //        },
            //        new OrderDetailsJson()
            //        {
            //            ItemName = "Show Piece",
            //            Price = 250.00f,
            //            Quantity = 3,
            //            Total = 750.00f
            //        }
            //    }
            //};

            //await _jsonUsingLinqService.InsertOrderDetailsAsync(orderWithOrderDetails);

            int PriceRandomizer = RandomIndex.Next(0, Prices.Count);
            int QuantityRandomizer = RandomIndex.Next(0, Prices.Count);
            int NameRandomizer = RandomIndex.Next(0, CustomerNames.Count);
            var OrderDetailsJson = new List<OrderDetailsJsonDto>()
            {
                new OrderDetailsJsonDto(3, Prices[PriceRandomizer],Quantities[QuantityRandomizer]),
                new OrderDetailsJsonDto(7, Prices[PriceRandomizer],Quantities[QuantityRandomizer])
            };

            var updateDetailJson = new OrderWithOrderDetailJsonUpdateDto(new Guid("26b12fd3-3459-4be2-af59-2d7b8f81ac8e"), CustomerNames[NameRandomizer], OrderDetailsJson);
            await _jsonUsingLinqService.UpdateOrderDetailsAsync(updateDetailJson);

        }
    }
}
