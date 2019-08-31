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

namespace TestOenTK
{
    internal static class Program
    {
        private static void Main(string[] args)
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

    class DrawData
    {
        public int VBO;
        public int VAO;
        public int IBO;
        public int offset;
        public Shader shader;
        public int enableAttrIndex;
    }

    internal class Game : GameWindow
    {
        DateTime m_StartTime;
        List<DrawData> m_DrawDatas = new List<DrawData>();

        private readonly float[] m_aVertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            1f, 0f, 0f, 1,

             -0.5f, 0.5f, 0.0f, //Bottom-right vertex
             0f, 1f, 0f, 1,

             0.5f,  0.5f, 0.0f,  //Top vertex
             0f, 0f, 1f, 1,

             0.5f, -0.5f, 0.0f,
             1f, 0.2f, 0.5f, 1,
        };

        private readonly uint[] m_aIndices = { // 注意索引从0开始! 
            0, 1, 3, // 第一个三角形
            1, 2, 3,  // 第二个三角形
        };

        private readonly float[] m_aVertices2 = {
            // pos              // color       // uv
            -0.5f, -0.5f, 0.0f, 0f, 1f, 0f, 1, 0, 0,

             -0.5f, 0.5f, 0.0f, 1f, 0f, 0f, 1, 0, 1,

             0.5f,  0.5f, 0.0f, 0f, 0f, 1f, 1, 1, 1,

             0.5f, -0.5f, 0.0f, 1f, 0.2f, 0.5f, 1, 0
        };

        private readonly uint[] m_aIndices2 = { // 注意索引从0开始! 
            0, 1, 3, // 第一个三角形
            1, 2, 3,  // 第二个三角形
        };

        private Shader m_Shader;
        private Shader m_Shader2;
        private int[] m_Tex = new int[2];
        


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
            m_StartTime = DateTime.Now;

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            //Code goes here
            m_Shader = new Shader("shader.vert", "shader.frag");
            m_Shader2 = new Shader("shader2.vert", "shader2.frag");

            Image<Rgb24> image = Image.Load<Rgb24>("container.jpg");

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Get an array of the pixels, in ImageSharp's internal format.
            Rgb24[] tempPixels = image.GetPixelSpan().ToArray();

            GL.GenTextures(2, m_Tex);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, m_Tex[0]);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, tempPixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            image.Dispose();

            Image<Rgba32> image2 = Image.Load<Rgba32>("awesomeface.png");

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            image2.Mutate(x => x.Flip(FlipMode.Vertical));

            //Get an array of the pixels, in ImageSharp's internal format.
            Rgba32[] tempPixels2 = image2.GetPixelSpan().ToArray();

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, m_Tex[1]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image2.Width, image2.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, tempPixels2);

            image2.Dispose();

            int[] VAO = new int[2];
            int[] IBO = new int[2];
            int[] VBO = new int[2];

            GL.GenBuffers(2, VBO);
            GL.GenBuffers(2, IBO);
            GL.GenVertexArrays(2, VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO[0]);

            GL.BufferData(BufferTarget.ArrayBuffer, m_aVertices.Length * sizeof(float), m_aVertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, m_aIndices.Length * sizeof(uint), m_aIndices, BufferUsageHint.StaticDraw);


            #region vao 1
            m_Shader.Use();
            
            GL.BindVertexArray(VAO[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO[0]);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);


            GL.BindVertexArray(0);

            DrawData data = new DrawData();
            data.VAO = VAO[0];
            data.VBO = VBO[0];
            data.IBO = IBO[0];
            data.offset = 0;
            data.shader = m_Shader;
            data.enableAttrIndex = 1;

            m_DrawDatas.Add(data);
            #endregion
#if true
            #region vao 2

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO[1]);

            GL.BufferData(BufferTarget.ArrayBuffer, m_aVertices2.Length * sizeof(float), m_aVertices2, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, m_aIndices2.Length * sizeof(uint), m_aIndices2, BufferUsageHint.StaticDraw);


            m_Shader2.Use();

            int iOurTexture1 = m_Shader2.GetUniformLocation("ourTexture");
            GL.Uniform1(iOurTexture1, m_Tex[0]);

            int iOurTexture2 = m_Shader2.GetUniformLocation("ourTexture2");
            GL.Uniform1(iOurTexture2, m_Tex[1]);

            GL.BindVertexArray(VAO[1]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO[1]);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 9 * sizeof(float), 7 * sizeof(float));

            GL.BindVertexArray(0);



            data = new DrawData();
            data.VAO = VAO[1];
            data.VBO = VBO[1];
            data.IBO = IBO[1];
            data.offset =12;
            data.shader = m_Shader2;
            data.enableAttrIndex = 2;

            m_DrawDatas.Add(data);
#endregion
#endif

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            foreach(var data in m_DrawDatas)
            {
                GL.DeleteVertexArray(data.VAO);
                GL.DeleteBuffer(data.VBO);
                GL.DeleteBuffer(data.IBO);
            }



            m_Shader.Dispose();
            m_Shader2.Dispose();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            float fElpaseSeconds = (float)((DateTime.Now - m_StartTime).TotalMilliseconds / 1000);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.

            for(int i = 0; i < m_DrawDatas.Count; ++i) 
            {
                if (i == 0 || i == 1)
                {
                    var data = m_DrawDatas[i];

                    //Console.WriteLine("draw vao " + data.VAO);

                    //for (int attrIndex = 0; attrIndex < data.enableAttrIndex; ++attrIndex)
                    //    GL.EnableVertexAttribArray(attrIndex);

                    data.shader.Use();

                    if (i == 1)
                    {
                        int iOurColorLocation = data.shader.GetUniformLocation("ourColor");

                        float fGreenColor = ((float)Math.Sin(fElpaseSeconds) + 1) / 8;
                        GL.Uniform4(iOurColorLocation, 0, fGreenColor, 0, 1);

                        int iOurTranslateLocation = data.shader.GetUniformLocation("ourTranslate");

                        float fTranslateY = (float)Math.Sin(fElpaseSeconds)/2;
                        GL.Uniform3(iOurTranslateLocation, 0, fTranslateY, 0);

                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, m_Tex[0]);

                        GL.ActiveTexture(TextureUnit.Texture1);
                        GL.BindTexture(TextureTarget.Texture2D, m_Tex[1]);

                    }

                    GL.BindVertexArray(data.VAO);

                    //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

                    GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, data.offset);
                    GL.BindVertexArray(0);
                }

            }


            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
