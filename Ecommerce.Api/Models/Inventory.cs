using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int QuantityInStock { get; set; }

    public DateTime LastUpdated { get; set; }
}
