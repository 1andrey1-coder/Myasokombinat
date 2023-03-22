using System;
using System.Collections.Generic;

namespace Myasokombinat;

public partial class TblManufacturer
{
    public int ManufacturerId { get; set; }

    public string? ManufacturerTitle { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
