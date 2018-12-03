using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TL_Feladat05
{
    class Program
    {
        public static int CONSUMERCOUNT = 1;
        public static int MAXCOLOR = 5;

        static void Main(string[] args)
        {
            Scheduler scheduler = new Scheduler(CONSUMERCOUNT);

        }

        
    }
}
