using System;
using System.Collections.Generic;

namespace Myasokombinat;

public partial class TblOrderStatus
{
    public int OrderStatusId { get; set; }

    public string? OrderStatusTitle { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
