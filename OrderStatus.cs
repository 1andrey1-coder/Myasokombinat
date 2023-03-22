using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myasokombinat
{
    public partial class OrderStatus
    {
        public int StatusId { get; set; }

        public string OrderStatus1 { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; } = new List<Order>();
    }

}
