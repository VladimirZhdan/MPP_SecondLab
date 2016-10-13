using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_WeakDelegate
{
    public class ListenerObject
    {
        public void Handler(int x)
        {
            Console.WriteLine("Handler(int)");
        }

        public void Handler(int x, double y)
        {
            Console.WriteLine("Handler(int, double)");
        }

        public void Handler(int x, double y, int z)
        {
            Console.WriteLine("Handler(int, double, int)");
        }

        public void Handler(int x0, int x1, int x2, int x3)
        {
            Console.WriteLine("Handler(int, int, int, int)");
        }

        public void Handler()
        {
            Console.WriteLine("Handler(void)");
        }
    }
}
