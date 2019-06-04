using Cool;
using Cool.Interface.GameEngine;
using Cool.Test.Engine;
using System;
using System.Threading;

namespace TestGameClient
{
    public class Program
    {
        static IDisplayEngine engine = new DisplayEngine();

        static void Main(string[] args)
        {
            engine.Init();

            IMain main = new Cool.GameClient.Main();
            main.Init(engine);

            Logger.Info("Press any key exit");
            while(Console.KeyAvailable == false)
            {
                engine.Time.CurrElapseMS += 16;
                engine.Time.DeltaMS = 16;

                try
                {
                    main.Update();
                }
                catch(Exception e)
                {
                    Logger.Error(e);
                }

                Thread.Sleep(16);
            }
        }
    }
}
