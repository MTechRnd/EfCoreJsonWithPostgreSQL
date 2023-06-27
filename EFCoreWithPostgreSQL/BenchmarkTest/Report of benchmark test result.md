

# Report
Here, you can see all query comparisons between the traditional approach and json linq approach. Here I have created two benchmark methods one is 
TraditionalBenchmark and the other one is JsonBenchmark. I have tested all queries one by one and made this report. You can see the test results as 
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
|      JsonBenchmark   | 47.19 ms | 1.699 ms | 0.263 ms |  0.53 |  272.7273 |        - |   3.25 MB |        0.13 |
| TraditionalBenchmark | 88.73 ms | 6.889 ms | 1.789 ms |  1.00 | 2833.3333 | 333.3333 |  25.61 MB |        1.00 |

 Performance Improving of Json query is 61.12%	

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
|      JsonBenchmark   | 194.8 us | 40.03 us | 10.40 us |  0.58 |    0.03 | 0.7324 |   8.29 KB |        0.63 |
| TraditionalBenchmark | 335.3 us |  8.35 us |  1.29 us |  1.00 |    0.00 | 0.9766 |  13.18 KB |        1.00 |

Performance Improving of Json query is 53.00%

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
|      JsonBenchmark   | 270.7 us | 38.55 us | 10.01 us |  0.59 |    0.02 | 1.4648 |  17.12 KB |        0.52 |
| TraditionalBenchmark | 467.4 us | 22.49 us |  3.48 us |  1.00 |    0.00 | 2.9297 |  33.17 KB |        1.00 |

Performance Improving of Json query is 53.30%

## Total orders for given customer

### Traditional Query
SELECT count(*)::int </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
WHERE o."Id" = @__id_0
                          
### Json Linq Query
SELECT jsonb_array_length("OrderDetailsJson") AS TotalOrderByCustomerId </br>
FROM "OrderWithOrderDetails" </br>
WHERE "OrderWithOrderDetails"."Id" = '372323ef-ba6b-4985-929c-951c8cd0d226'

### Benchmark Test Result:
|               Method |     Mean |    Error |   StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
|      JsonBenchmark   | 522.8 us | 39.32 us | 10.21 us |  0.98 |    0.02 | 1.9531 |  18.13 KB |        1.12 |
| TraditionalBenchmark | 531.4 us | 67.20 us | 17.45 us |  1.00 |    0.00 | 0.9766 |  16.19 KB |        1.00 |

Performance Improving of Json query is 1.63%

## Total orders for all customer

### Traditional Query
SELECT o."Id", count(*)::int AS "TotalOrder" </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
GROUP BY o."Id"

### Json Linq Query
SELECT ""Id"", jsonb_array_length(""OrderDetailsJson"") AS TotalOrder
FROM ""OrderWithOrderDetails""

### Benchmark Test Result:
|               Method |      Mean |     Error |    StdDev | Ratio |     Gen0 |     Gen1 |    Gen2 | Allocated | Alloc Ratio |
|--------------------- |----------:|----------:|----------:|------:|---------:|---------:|--------:|----------:|------------:|
|      JsonBenchmark   |  4.041 ms | 0.7985 ms | 0.2074 ms |  0.23 | 281.2500 | 132.8125 | 39.0625 |   2.49 MB |        1.00 |
| TraditionalBenchmark | 17.951 ms | 0.3463 ms | 0.0899 ms |  1.00 | 281.2500 | 156.2500 | 31.2500 |   2.48 MB |        1.00 |

Performance Improving of Json query is 126.50%

## Average of all price 

### Traditional Query
SELECT avg(o0."Price")::real </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId"

### Json Linq Query
SELECT AVG(CAST(json_data ->> 'Price' AS real)) AS AverageOfPrice </br>
FROM ""OrderWithOrderDetails"", </br>
jsonb_array_elements(""OrderWithOrderDetails"".""OrderDetailsJson"") AS json_data

### Benchmark Test Result:
|               Method |     Mean |     Error |   StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------- |---------:|----------:|---------:|------:|--------:|----------:|------------:|
| TraditionalBenchmark | 12.47 ms |  0.904 ms | 0.140 ms |  1.00 |    0.00 |  13.21 KB |        1.00 |
|      JsonBenchmark   | 43.88 ms | 20.898 ms | 5.427 ms |  3.57 |    0.52 |  17.78 KB |        1.35 |

Performance Improving of Traditional query is 111.82%

## Maximum quantity by order id

### Traditional Query
SELECT min(o0."Quantity") </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
WHERE o."Id" = @__id_0

### Json Linq Query
SELECT MAX(CAST(json_data ->> 'Quantity' AS Integer)) AS MaximumQuantity </br>
FROM "OrderWithOrderDetails", </br>
jsonb_array_elements("OrderWithOrderDetails"."OrderDetailsJson") AS json_data </br>
WHERE "OrderWithOrderDetails"."Id" = '372323ef-ba6b-4985-929c-951c8cd0d226'

### Benchmark Test Result:
|               Method |     Mean |    Error |  StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
| TraditionalBenchmark | 512.1 us | 36.15 us | 9.39 us |  1.00 |    0.00 | 0.9766 |  16.63 KB |        1.00 |
|        JsonBenchmark | 669.1 us | 28.29 us | 7.35 us |  1.31 |    0.02 | 1.9531 |  20.83 KB |        1.25 |

Performance Improving of Traditional query is 26.58%

## Total by order id

### Traditional Query
SELECT COALESCE(sum(o0."Total"), 0) </br>
FROM "Orders" AS o </br>
INNER JOIN "OrderDetails" AS o0 ON o."Id" = o0."OrderId" </br>
WHERE o."Id" = @__id_0

### Json Linq Query
SELECT SUM(CAST(json_data ->> 'Total' AS real)) AS TotalByOrderId </br>
FROM "OrderWithOrderDetails", </br>
jsonb_array_elements("OrderWithOrderDetails"."OrderDetailsJson") AS json_data </br>
WHERE "OrderWithOrderDetails"."Id" = '372323ef-ba6b-4985-929c-951c8cd0d226'

### Benchmark Test Result:
|               Method |     Mean |    Error |  StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |---------:|---------:|--------:|------:|-------:|----------:|------------:|
| TraditionalBenchmark | 508.1 us | 15.25 us | 3.96 us |  1.00 | 0.9766 |  16.77 KB |        1.00 |
|        JsonBenchmark | 730.2 us | 18.08 us | 2.80 us |  1.44 | 1.9531 |  21.19 KB |        1.26 |

Performance Improving of Traditional query is 35.87%

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
|    JsonBenchmark     | 10.98 ms |  6.093 ms | 1.582 ms |  0.60 |    0.02 | 1240.2344 | 181.6406 |  11.14 MB |        0.72 |
| TraditionalBenchmark | 18.34 ms | 10.329 ms | 2.682 ms |  1.00 |    0.00 | 1726.5625 | 140.6250 |   15.5 MB |        1.00 |

Performance Improving of Json query is 50.20%
 
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
|               Method |       Mean |     Error |   StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------- |-----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
|        JsonBenchmark |   570.4 us |  25.88 us |  6.72 us |  0.57 |    0.02 | 2.9297 |   31.5 KB |        0.46 |
| TraditionalBenchmark | 1,002.4 us | 128.44 us | 33.36 us |  1.00 |    0.00 | 5.8594 |  67.87 KB |        1.00 |

Performance Improving of Json query is 54.93%