using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public virtual ICollection<ProductStore> ProductStores { get; } = new List<ProductStore>();
}
