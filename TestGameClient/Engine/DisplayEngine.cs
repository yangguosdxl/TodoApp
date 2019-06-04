using Cool.Interface.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Test.Engine
{
    class DisplayEngine : IDisplayEngine
    {

        public IScreen Screen { get ; private set ; }
        public ISceneManager SceneManager { get ; private set; }
        public ITime Time { get ; private set; }

        public void Init()
        {
            Screen = new Screen();
            SceneManager = new SceneManager();
            Time = new Time();
        }
    }
}
