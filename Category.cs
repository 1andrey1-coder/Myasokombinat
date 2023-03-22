using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myasokombinat
{
    public partial class Category
    {
        public int CategoryId { get; set; }

        public string ProductCategory { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
    
}
