using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myasokombinat
{
    internal class DB
    {

        private static MyasokombinatContext Instance;
        public static MyasokombinatContext  GetInstance()
        {
            if (Instance == null)
                Instance = new MyasokombinatContext();
            return Instance;
        }
        

    }
}



