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
            sourceObject.Completed0 += (Action)new WeakDelegateModified((Action)listenerObject.Handler).Weak;
            sourceObject.Completed += (Action<int>)new WeakDelegateModified((Action<int>)listenerObject.Handler).Weak;
            sourceObject.Completed1 += (Action<int, double>)new WeakDelegateModified((Action<int, double>)listenerObject.Handler).Weak;
            sourceObject.Completed2 += (Action<int, double, int>)new WeakDelegateModified((Action<int, double, int>)listenerObject.Handler).Weak;
            sourceObject.Completed3 += (Action<int, int, int, int>)new WeakDelegateModified((Action<int, int, int, int>)listenerObject.Handler).Weak;
            //sourceObject.Completed += (Action<int>)new WeakDelegateModified((Action<int>)listenerObject.Handler).Weak;
            //sourceObject.Completed1 += (Action<int, double>)new WeakDelegate((Action<int, double>)listenerObject.Handler).Weak;
            //sourceObject.Completed2 += (Action<int, double, int>)new WeakDelegate((Action<int, double, int>)listenerObject.Handler).Weak;
            //sourceObject.Completed3 += (Action<int, int, int, int>)new WeakDelegate((Action<int, int, int, int>)listenerObject.Handler).Weak;
            //listenerObject = null;
            //GC.Collect(2, GCCollectionMode.Forced);
            //GC.WaitForFullGCComplete(300);
            //GC.WaitForPendingFinalizers();
            sourceObject.CallAllEvents();
        }
    }
}
