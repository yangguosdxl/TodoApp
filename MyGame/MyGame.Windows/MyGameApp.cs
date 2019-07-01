using Xenko.Engine;

namespace MyGame.Windows
{
    class MyGameApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
