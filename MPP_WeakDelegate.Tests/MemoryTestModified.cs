using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MPP_WeakDelegate;

namespace MPP_WeakDelegate.Tests
{
    /// <summary>
    /// Class for tesing WeakDelegateModified class
    /// </summary>
    [TestClass]
    public class MemoryTestModified
    {
        [TestMethod]
        public void CheckDecreaseMemory()
        {
            SourceObject sourceObject = new SourceObject();
            ListenerObject[] listenerObject = new ListenerObject[10];
            for (int i = 0; i < 10; i++)
            {
                listenerObject[i] = new ListenerObject();
                sourceObject.Completed += (Action<int>)new WeakDelegateModified((Action<int>)listenerObject[i].Handler).Weak;
                sourceObject.Completed1 += (Action<int, double>)new WeakDelegateModified((Action<int, double>)listenerObject[i].Handler).Weak;
                sourceObject.Completed2 += (Action<int, double, int>)new WeakDelegateModified((Action<int, double, int>)listenerObject[i].Handler).Weak;
                sourceObject.Completed3 += (Action<int, int, int, int>)new WeakDelegateModified((Action<int, int, int, int>)listenerObject[i].Handler).Weak;
            }

            long initialMemoryCount = GC.GetTotalMemory(true);
            for (int i = 0; i < 10; i++)
            {
                listenerObject[i] = null;
            }
            GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete(300);
            GC.WaitForPendingFinalizers();

            long currentMemoryCount = GC.GetTotalMemory(true);

            bool expectedResult = true;

            bool actualResult = (currentMemoryCount < initialMemoryCount);

            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void CheckDeadOfWeakReference()
        {
            SourceObject sourceObject = new SourceObject();
            ListenerObject listenerObject = new ListenerObject();
            WeakDelegateModified weakDelegate = new WeakDelegateModified((Action<int>)listenerObject.Handler);
            sourceObject.Completed += (Action<int>)weakDelegate.Weak;
            listenerObject = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            Assert.AreEqual(false, weakDelegate.TargetRef.IsAlive);
        }
        

    }
}
