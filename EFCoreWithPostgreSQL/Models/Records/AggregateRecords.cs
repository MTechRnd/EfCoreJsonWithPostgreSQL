using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreJsonApp.Models.Records
{
    public record AverageOfPriceResult(decimal AverageOfPrice);
    public record AverageOfQuantityResult(float AverageOfQuantity);
    public record MaxPriceResult(float MaximumPrice);
    public record MaxQuantityResult(int MaximumQuantity);
    public record MinPriceResult(float MinimumPrice);
    public record MinQuantityResult(int MinimumQuantity);
    public record TotalByOrderResult(float TotalByOrderId);
    public record TotalOrderByCustomerResult(int TotalOrderByCustomerId);
    public record TotalPriceResult(double TotalPrice);
    public record TotalQuantityResult(int TotalQuantity);

}
