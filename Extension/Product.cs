﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Myasokombinat
{
    public partial class Product
    {
        public Brush ProductBackground
        {
            get
            {
                if (ProductQuantityInStock == 0)
                    return Brushes.Gray;
                else
                    return Brushes.White;
            }
        }
    }
}
