using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string StreatName { get; set; } = null!;

    public virtual ICollection<ProductStore> ProductStores { get; } = new List<ProductStore>();
}
