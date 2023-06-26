# EfCoreJsonWithPostgreSQL
Using EFCore Json with PostgreSQL Database

The following step, you need to opt for when you clone this repository:
1. You need to change the URL of the Postgre SQL database.
    For that you need to set URL in secret file my connection string of the Postgre SQL database is : </br>
    {
      "ConnectionStrings": {
        "DefaultConnection": "User ID=postgres;Password=1234;Server=localhost;Database=OrdersDB;Port=5432; IntegratedSecurity=true;Pooling=true;"
      }
    }
2. You need to just run the update-database command when you clone this repo as follows:
- Update-Database -context DataContext
- Update-Database -context JsonDataContext
- Maybe you need to run this SQL query in Postgres (CREATE EXTENSION IF NOT EXISTS "uuid-ossp";)
- After running this run the above update-database command again.
- When you run the above commands database will be created and also three tables will be created and you get some data inside it.

The following are basic commands which I have used:
- Add-Migration "migration for traditional schema" -context DataContext
- Add-Migration "migration for json column schema" -context JsonDataContext
- Update-Database -context DataContext
- Update-Database -context JsonDataContext

## Code Summary:
- There are two services. One service is for traditional queries and the second service is for json service.
- For the traditional approach, there are two tables. One is Order where Id is the primary key. The second table is OrderDetails where OrderId is a foreign key.
- For json approach, there is only one table which is orderWithOrderDetails. Where you can find one column which has property jsonb. Here in this column, I have stored data in an array of json(OrderDetails).

### ER Diagrams:
- Relationship between order and orderDetails with data type. </br>
![image](https://github.com/MTechRnd/EfCoreJsonWithPostgreSQL/assets/123544692/b8163da4-fbb9-427f-af40-1939fe81b34b)

- OrderWithOrderDetails table column name with Data Type. </br>
![image](https://github.com/MTechRnd/EfCoreJsonWithPostgreSQL/assets/123544692/f6684a2a-0cc4-46d2-8054-d5b2f0ec9abe)

## Benchmark Test Report Link is as follows:
https://github.com/MTechRnd/EfCoreJsonWithPostgreSQL/blob/benchmarkTest/EFCoreWithPostgreSQL/BenchmarkTest/Report%20of%20benchmark%20test%20result.md


