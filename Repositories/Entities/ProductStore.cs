using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class ProductStore
{
    public int ProductStoreId { get; set; }

    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
