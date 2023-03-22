using System;
using System.Collections.Generic;

namespace Myasokombinat;

public partial class TblCategory
{
    public int CategoryId { get; set; }

    public string CategoryTitle { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
