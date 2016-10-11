using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MPP_WeakDelegate;

namespace MPP_WeakDelegate.Tests
{
    [TestClass]
    public class MemoryTest
    {
        [TestMethod]
        public void CheckMemory()
        {
            SourceObject sourceObject = new SourceObject();
            ListenerObject[] listenerObject = new ListenerObject[10];
            for (int i = 0; i < 10; i++ )
            {
                listenerObject[i] = new ListenerObject();
                sourceObject.Completed += (Action<int>)new WeakDelegate((Action<int>)listenerObject[i].Handler).Weak;
                sourceObject.Completed1 += (Action<int, double>)new WeakDelegate((Action<int, double>)listenerObject[i].Handler).Weak;
                sourceObject.Completed2 += (Action<int, double, int>)new WeakDelegate((Action<int, double, int>)listenerObject[i].Handler).Weak;
                sourceObject.Completed3 += (Action<int, int, int, int>)new WeakDelegate((Action<int, int, int, int>)listenerObject[i].Handler).Weak;
            }
            


            long initialMemoryCount = GC.GetTotalMemory(true);
            for (int i = 0; i < 10; i++)
            {
                listenerObject[i] = null;
            } 
            GC.Collect(2, GCCollectionMode.Forced);      
            Thread.Sleep(300);
            GC.WaitForFullGCComplete(100);
            GC.WaitForPendingFinalizers();

            long currentMemoryCount = GC.GetTotalMemory(true);

            bool expectedResult = true;

            

            bool actualResult = (currentMemoryCount < initialMemoryCount);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
