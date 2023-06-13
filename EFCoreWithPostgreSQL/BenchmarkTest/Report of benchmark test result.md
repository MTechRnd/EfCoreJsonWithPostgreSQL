

# Report
Here, you can see all query comparisons between the traditional approach and json linq approach. Here I have created two benchmark methods one is 
TraditionalBenchmark and the other one is JsonLinqBenchmark. I have tested all queries one by one and made this report. You can see the test results as 
follow. I have also added a performance-improving percentage so you can easily see which performed better for that case.

# All Queries

## Get all data

### Traditional Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o.xmin, o."UpdatedAt", o0."Id", o0."CreatedAt", o0."ItemName", o0."OrderId", o0."Price", o0."Quantity", o0.xmin, o0."Total", o0."UpdatedAt" </br>
FROM "Orders" AS o </br>
LEFT JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
ORDER BY o."Id"

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o

### Benchmark Test Result:
|               Method |     Mean |    Error |   StdDev | Ratio |      Gen0 |     Gen1 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|---------:|------:|----------:|---------:|----------:|------------:|
|    JsonLinqBenchmark | 47.19 ms | 1.699 ms | 0.263 ms |  0.53 |  272.7273 |        - |   3.25 MB |        0.13 |
| TraditionalBenchmark | 88.73 ms | 6.889 ms | 1.789 ms |  1.00 | 2833.3333 | 333.3333 |  25.61 MB |        1.00 |

 Performance Improving of JsonLinq query is 61.12%	

## Get single data of customer

### Traditional Query
SELECT t."Id", t."CreatedAt", t."CustomerName", t."OrderDate", t.xmin, t."UpdatedAt", o0."Id", o0."CreatedAt", o0."ItemName", o0."OrderId", o0."Price", o0."Quantity", o0.xmin, o0."Total", o0."UpdatedAt" </br>
FROM ( </br>
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o.xmin, o."UpdatedAt" </br>
FROM "Orders" AS o </br>
WHERE o."Id" = @__id_0 </br>
LIMIT 1 </br>
) AS t </br>
LEFT JOIN "OrderDetails" AS o0 ON t."Id" = o0."OrderId" </br>
ORDER BY t."Id"

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o </br>
WHERE o."Id" = @__id_0 </br>
LIMIT 1
### Benchmark Test Result:
|               Method |     Mean |    Error |   StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
|    JsonLinqBenchmark | 194.8 us | 40.03 us | 10.40 us |  0.58 |    0.03 | 0.7324 |   8.29 KB |        0.63 |
| TraditionalBenchmark | 335.3 us |  8.35 us |  1.29 us |  1.00 |    0.00 | 0.9766 |  13.18 KB |        1.00 |

Performance Improving of JsonLinq query is 53.00%

## Get data for multiple customer

### Traditional Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o.xmin, o."UpdatedAt", o0."Id", o0."CreatedAt", o0."ItemName", o0."OrderId", o0."Price", o0."Quantity", o0.xmin, o0."Total", o0."UpdatedAt" </br>
FROM "Orders" AS o </br>
LEFT JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
WHERE o."Id" IN ('003caf85-8c76-4fb6-9140-18bc3ed523aa', 'b6fd1215-41f5-ed11-9f05-f46b8c8f0ef6', '01fa1215-41f5-ed11-9f05-f46b8c8f0ef6') </br>
ORDER BY o."Id"

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o </br>
WHERE o."Id" IN ('002b1a62-72a1-483c-8b3b-40f7c8283bdf', '977827d2-19fa-ed11-9f08-f46b8c8f0ef6', '708b27d2-19fa-ed11-9f08-f46b8c8f0ef6')

### Benchmark Test Result:
|               Method |     Mean |    Error |   StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
|    JsonLinqBenchmark | 270.7 us | 38.55 us | 10.01 us |  0.59 |    0.02 | 1.4648 |  17.12 KB |        0.52 |
| TraditionalBenchmark | 467.4 us | 22.49 us |  3.48 us |  1.00 |    0.00 | 2.9297 |  33.17 KB |        1.00 |

Performance Improving of JsonLinq query is 53.30%

## Total orders for given customer

### Traditional Query
 SELECT count(*)::int
FROM "Orders" AS o
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId"
WHERE o."Id" = @__id_0
                          
### Json Linq Query
SELECT [o].[Id], [o].[CreatedAt], [o].[CustomerName], [o].[OrderDate], [o].[Timestamp], [o].[UpdatedAt], JSON_QUERY([o].[OrderDetailsJson],'$')  </br>
FROM [OrderWithOrderDetails] AS [o]  </br>
WHERE [o].[Id] = @__id_0  </br>

### Benchmark Test Result:
|               Method |     Mean |    Error |  StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
|    JsonLinqBenchmark | 132.0 us | 32.49 us | 8.44 us |  0.51 |    0.03 | 0.6104 |   5.62 KB |        0.46 |
| TraditionalBenchmark | 257.2 us | 23.78 us | 6.18 us |  1.00 |    0.00 | 0.9766 |  12.16 KB |        1.00 |

Performance Improving of JsonLinq query is 64.33%

## Total orders for all customer

### Traditional Query
SELECT o."Id", count(*)::int AS "TotalOrder" </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
GROUP BY o."Id"

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt"
FROM "OrderWithOrderDetails" AS o

### Benchmark Test Result:
|               Method |     Mean |    Error |   StdDev | Ratio | RatioSD |     Gen0 |     Gen1 |    Gen2 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|---------:|------:|--------:|---------:|---------:|--------:|----------:|------------:|
| TraditionalBenchmark | 17.72 ms | 0.954 ms | 0.148 ms |  1.00 |    0.00 | 281.2500 | 156.2500 | 31.2500 |   2.48 MB |        1.00 |
|    JsonLinqBenchmark | 46.50 ms | 4.453 ms | 0.689 ms |  2.62 |    0.04 | 272.7273 |  90.9091 |       - |   3.08 MB |        1.24 |

Performance Improving of Traditional query is 89.62%

## Average of all price 

### Traditional Query
 SELECT avg(o0."Price")::real </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId"

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o

### Benchmark Test Result:
|               Method |     Mean |    Error |   StdDev | Ratio | RatioSD |     Gen0 |  Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|---------:|------:|--------:|---------:|-----------:|------------:|
| TraditionalBenchmark | 11.68 ms | 0.140 ms | 0.022 ms |  1.00 |    0.00 |        - |   10.24 KB |        1.00 |
|    JsonLinqBenchmark | 47.65 ms | 3.726 ms | 0.968 ms |  4.10 |    0.07 | 272.7273 | 3127.83 KB |      305.56 |

Performance Improving of Traditional query is 121.25%

## Maximum quantity by order id

### Traditional Query
SELECT min(o0."Quantity")
FROM "Orders" AS o
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId"
WHERE o."Id" = @__id_0

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o </br>
WHERE o."Id" = @__id_0

### Benchmark Test Result:
|               Method |     Mean |    Error |  StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
|    JsonLinqBenchmark | 122.3 us | 24.50 us | 3.79 us |  0.45 |    0.02 | 0.4883 |   5.73 KB |        0.44 |
| TraditionalBenchmark | 269.0 us | 15.90 us | 4.13 us |  1.00 |    0.00 | 0.9766 |  12.92 KB |        1.00 |

Performance Improving of JsonLinq query is 74.98%

## Total by order id

### Traditional Query
SELECT COALESCE(sum(o0."Total"), 0) </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
WHERE o."Id" = @__id_0

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o </br>
WHERE o."Id" = @__id_0

### Benchmark Test Result:
|               Method |     Mean |    Error |  StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
|    JsonLinqBenchmark | 132.5 us | 36.60 us | 9.51 us |  0.50 |    0.04 | 0.4883 |   5.73 KB |        0.44 |
| TraditionalBenchmark | 264.0 us | 18.24 us | 2.82 us |  1.00 |    0.00 | 0.9766 |  13.02 KB |        1.00 |

Performance Improving of JsonLinq query is 66.33%

## Insert Data

### Traditional Query
INSERT INTO "OrderDetails" ("CreatedAt", "ItemName", "OrderId", "Price", "Quantity", "UpdatedAt") </br>
 VALUES (@p4, @p5, @p6, @p7, @p8, @p9) </br>
 RETURNING "Id", xmin, "Total"; </br>
 INSERT INTO "OrderDetails" ("CreatedAt", "ItemName", "OrderId", "Price", "Quantity", "UpdatedAt") </br>
 VALUES (@p10, @p11, @p12, @p13, @p14, @p15) </br>
 RETURNING "Id", xmin, "Total";

### Json Linq Query
INSERT INTO "OrderWithOrderDetails" ("CreatedAt", "CustomerName", "OrderDate", "OrderDetailsJson", "UpdatedAt") </br>
VALUES (@p0, @p1, @p2, @p3, @p4) </br>
RETURNING "Id", xmin;

### Benchmark Test Result:
|               Method |     Mean |     Error |   StdDev | Ratio | RatioSD |      Gen0 |     Gen1 | Allocated | Alloc Ratio |
|--------------------- |---------:|----------:|---------:|------:|--------:|----------:|---------:|----------:|------------:|
|    JsonLinqBenchmark | 10.98 ms |  6.093 ms | 1.582 ms |  0.60 |    0.02 | 1240.2344 | 181.6406 |  11.14 MB |        0.72 |
| TraditionalBenchmark | 18.34 ms | 10.329 ms | 2.682 ms |  1.00 |    0.00 | 1726.5625 | 140.6250 |   15.5 MB |        1.00 |

Performance Improving of JsonLinq query is 50.20%
 
## Updating Data

### Traditional Query
UPDATE "OrderDetails" SET "Price" = @p0, "Quantity" = @p1, "UpdatedAt" = @p2 </br>
WHERE "Id" = @p3 AND xmin = @p4 </br>
RETURNING xmin, "Total"; </br>
UPDATE "Orders" SET "CustomerName" = @p5, "UpdatedAt" = @p6 </br>
WHERE "Id" = @p7 AND xmin = @p8 </br>
RETURNING xmin;

### Json Linq Query
SELECT o."Id", o."CreatedAt", o."CustomerName", o."OrderDate", o."OrderDetailsJson", o.xmin, o."UpdatedAt" </br>
FROM "OrderWithOrderDetails" AS o </br>
WHERE o."Id" = @__orderWithOrdderDetailsDto_Id_0 </br>
LIMIT 1 </br>
UPDATE "OrderWithOrderDetails" SET "CustomerName" = @p0, "OrderDetailsJson" = @p1, "UpdatedAt" = @p2 </br>
WHERE "Id" = @p3 AND xmin = @p4 </br>
RETURNING xmin;


### Benchmark Test Result:
|               Method |        Mean |       Error |    StdDev | Ratio | RatioSD |      Gen0 |     Gen1 |   Allocated | Alloc Ratio |
|--------------------- |------------:|------------:|----------:|------:|--------:|----------:|---------:|------------:|------------:|
| TraditionalBenchmark |    979.0 us |    72.64 us |  11.24 us |  1.00 |    0.00 |    5.8594 |        - |    67.89 KB |        1.00 |
|    JsonLinqBenchmark | 71,306.4 us | 1,595.72 us | 414.40 us | 72.70 |    0.77 | 2250.0000 | 500.0000 | 20869.86 KB |      307.43 |

Performance Improving of Traditional query is 194.06%
