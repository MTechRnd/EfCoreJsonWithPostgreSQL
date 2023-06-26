using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreJsonApp.Models.OrderDetails
{
    public record OrderDetailUpdateDto(Guid Id, float Price, int Quantity);
}
