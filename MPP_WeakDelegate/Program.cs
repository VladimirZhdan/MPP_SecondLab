using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_WeakDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            SourceObject sourceObject = new SourceObject();
            ListenerObject listenerObject = new ListenerObject();
            sourceObject.Completed += (Action<int>)new WeakDelegate((Action<int>)listenerObject.Handler).Weak;
            sourceObject.Completed1 += (Action<int, double>)new WeakDelegate((Action<int, double>)listenerObject.Handler).Weak;
            sourceObject.Completed2 += (Action<int, double, int>)new WeakDelegate((Action<int, double, int>)listenerObject.Handler).Weak;
            sourceObject.Completed3 += (Action<int, int, int, int>)new WeakDelegate((Action<int, int, int, int>)listenerObject.Handler).Weak;
            sourceObject.CallAllEvents();
        }
    }
}
