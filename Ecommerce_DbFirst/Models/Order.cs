using System;
using System.Collections.Generic;

namespace Ecommerce_DBFirst.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }
}
