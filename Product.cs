using System;
using System.Collections.Generic;

namespace Myasokombinat;

public partial class Product
{
    public string ProductArticleNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal ProductUnit { get; set; }

    public int ProductCost { get; set; }

    public int ProductDiscountAmount { get; set; }

    public int ProductManufacturer { get; set; }

    public int ProductProvider { get; set; }

    public int ProductCategory { get; set; }

    public int? ProductStatus { get; set; }

    public int ProductQuantityInStock { get; set; }

    public string ProductDescription { get; set; } = null!;

    public byte[]? ProductPhoto { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual TblCategory ProductCategoryNavigation { get; set; } = null!;

    public virtual TblManufacturer ProductManufacturerNavigation { get; set; } = null!;

    public virtual TblProvider ProductProviderNavigation { get; set; } = null!;
}
