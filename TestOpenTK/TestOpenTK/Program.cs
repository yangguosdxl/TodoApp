using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

//using OpenTK.Graphics.ES20;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TestOpenTK;

namespace TestOenTK
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Curr work path: " + Directory.GetCurrentDirectory());
            //using (Game game = new Game(800, 600, "LearnOpenTK"))
            using(GameWindow gw = new TriangleGameWindow(800,600, "TriangleGameWindow"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                gw.Run(60.0);
            }

        }
    }

    
}
