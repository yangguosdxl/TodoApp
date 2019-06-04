using Cool.Interface.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Test.Engine
{
    class Time : ITime
    {
        public int DeltaMS { get ; set ; }
        public long CurrElapseMS { get ; set ; }
    }
}
