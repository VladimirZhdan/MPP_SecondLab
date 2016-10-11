﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_WeakDelegate
{
    class SourceObject
    {
        public event Action<int> Completed;
        public event Action<int, double> Completed1;
        public event Action<int, double, int> Completed2;
        public event Action<int, int, int, int> Completed3;

        public void CallAllEvents()
        {
            if(Completed != null)
            {
                Completed(1);
            }
            if(Completed1 != null)
            {
                Completed1(1, 1.1);
            }
            if(Completed2 != null)
            {
                Completed2(1, 1.1, 1);
            }
            if(Completed3 != null)
            {
                Completed3(1, 1, 1, 1);
            }                                    
        }
    }
}
