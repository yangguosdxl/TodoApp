using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using OpenTK.Input;
using System;
using System.IO;

namespace TestOenTK
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Curr work path: " + Directory.GetCurrentDirectory());
            using (Game game = new Game(800, 600, "LearnOpenTK"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }
        }
    }

    class Game : GameWindow
    {
        int VertexBufferObject;

        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };

        Shader shader;

        public Game(int width, int height, string title) : 
            base(width, height, GraphicsMode.Default, title)
        { }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Get the state of the keyboard this frame
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //Code goes here
            shader = new Shader("shader.vert", "shader.frag");
            VertexBufferObject = GL.GenBuffer();


            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);

            shader.Dispose();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader.Use();
            


            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
