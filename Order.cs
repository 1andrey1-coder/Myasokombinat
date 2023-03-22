using System;
using System.Collections.Generic;

namespace Myasokombinat;

public partial class Order
{
    public int OrderId { get; set; }

    public string? OrderNumber { get; set; }

    public string? OrderCompound { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime OrderDeliveryDate { get; set; }

    public int OrderPickupPoint { get; set; }

    public string? ClientFullName { get; set; }

    public int? OrderCode { get; set; }

    public int OrderStatus { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual TblOrderStatus OrderStatusNavigation { get; set; } = null!;
}
