using Cool.Interface.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Test.Engine
{
    class Screen : IScreen
    {
        public int Width { get ; set ; }
        public int Height { get ; set ; }
        public int DPI { get ; set ; }
    }
}
