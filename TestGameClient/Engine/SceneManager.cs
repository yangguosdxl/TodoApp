using Cool.Coroutine;
using Cool.Interface.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Test.Engine
{
    class SceneManager : ISceneManager
    {
        public void ChangeScene(string szSceneName, bool bAdditive)
        {
            Logger.Trace("Change to scene {0}", szSceneName);
        }

        public async MyTask ChangeSceneAsync(string szSceneName, bool bAdditive)
        {
            Logger.Trace("Change to scene {0}", szSceneName);
        }

        public void ChangeScnene(int iSceneTblID)
        {
            Logger.Trace("Change to scene {0}", iSceneTblID);
        }

        public async MyTask ChangeScneneAsync(int iSceneTblID)
        {
            Logger.Trace("Change to scene {0}", iSceneTblID);
        }
    }
}
