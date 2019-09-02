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
        private Texture m_Texture1;
        private Texture m_Texture2;

        private Matrix4 m_ModelTransform;
        private Matrix4 m_Local2World;
        private Matrix4 m_World2View;
        private Matrix4 m_View2Proj;

        private float m_fSpeed = 600;

        private Camera m_Camera = new Camera(new Vector3(0, 0, -2f), new Vector3(0, 0, 1f), new Vector3(0, 1, 0));

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
            else if (input.IsKeyDown(Key.Number1))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            else if (input.IsKeyDown(Key.Number2))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else if (input.IsKeyDown(Key.W))
            {
                Vector3 v = (float)(UpdateTime * m_fSpeed) * m_Camera.CameraFront;
                m_Camera.CameraPos += v;
            }
            else if (input.IsKeyDown(Key.S))
            {
                Vector3 v = (float)(UpdateTime * -m_fSpeed) * m_Camera.CameraFront;
                m_Camera.CameraPos += v;
            }
            else if (input.IsKeyDown(Key.A))
            {
                Vector3 v = (float)(UpdateTime * -m_fSpeed) * m_Camera.CameraRight;
                m_Camera.CameraPos += v;
            }
            else if (input.IsKeyDown(Key.D))
            {
                Vector3 v = (float)(UpdateTime * m_fSpeed) * m_Camera.CameraRight;
                m_Camera.CameraPos += v;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            m_StartTime = DateTime.Now;

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            

            //Code goes here
            m_Shader = new Shader("shader.vert", "shader.frag");
            m_Shader2 = new Shader("shader2.vert", "shader2.frag");

            m_Texture1 = new Texture("container.jpg", TextureUnit.Texture0);
            //m_Texture1.Use();
            m_Texture2 = new Texture("awesomeface.png", TextureUnit.Texture1);
            //m_Texture2.Use(TextureUnit.Texture1);

            m_ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90));
            m_Local2World = Matrix4.Identity;
            m_World2View = Matrix4.CreateTranslation(0, 0, -2);
            m_View2Proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 800.0f / 600, 0.1f, 100f);


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
            GL.Uniform1(iOurTexture1, 0);

            int iOurTexture2 = m_Shader2.GetUniformLocation("ourTexture2");
            GL.Uniform1(iOurTexture2, 1);

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

                    GL.BindVertexArray(data.VAO);

                    if (i == 1)
                    {
                        m_Texture1.Use();

                        m_Texture2.Use(TextureUnit.Texture1);
                    }

                    data.shader.Use();

                    if (i == 1)
                    {
                        int iOurColorLocation = data.shader.GetUniformLocation("ourColor");

                        float fGreenColor = ((float)Math.Sin(fElpaseSeconds) + 1) / 8;
                        GL.Uniform4(iOurColorLocation, 0, fGreenColor, 0, 1);

                        

                        float fTranslateY = (float)((Math.Sin(fElpaseSeconds) + 1) / 2 * 2*Math.PI);
                        m_ModelTransform = Matrix4.CreateRotationX(-fElpaseSeconds);
                        //m_ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90));

                        m_World2View = m_Camera.LookAt();

                        Matrix4 m = m_View2Proj * m_World2View * m_Local2World * m_ModelTransform;
                        // * m_ModelTransform;
                        m = m_ModelTransform * m_Local2World * m_World2View * m_View2Proj;
                        //m = m_ModelTransform * m_View2Proj;
                        //m = Matrix4.Identity;


                        int pvm = data.shader.GetUniformLocation("pvm");
                        GL.UniformMatrix4(pvm, false, ref m);

                        //m_ModelTransform = Matrix4.Identity;
                        //m_World2View = Matrix4.Identity;
                        //m_View2Proj = Matrix4.Identity;

                        //int model = data.shader.GetUniformLocation("model");
                        //GL.UniformMatrix4(model, false, ref m_ModelTransform);

                        //int view = data.shader.GetUniformLocation("view");
                        //GL.UniformMatrix4(view, false, ref m_World2View);

                        //int proj = data.shader.GetUniformLocation("proj");
                        //GL.UniformMatrix4(proj, false, ref m_View2Proj);

                    }


                    //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

                    GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, data.offset);

                    //if (i == 1)
                    //{
                    //    int iOurColorLocation = data.shader.GetUniformLocation("ourColor");

                    //    float fRedColor = ((float)Math.Sin(fElpaseSeconds) + 1) / 8;
                    //    GL.Uniform4(iOurColorLocation, fRedColor, 0, 0, 1);

                    //    int iOurTranslateLocation = data.shader.GetUniformLocation("ourTranslate");

                    //    float fTranslateY = (float)(Math.Sin(fElpaseSeconds) + 1) / 2;

                    //    Matrix4 m = Matrix4.CreateTranslation(-1f, 0f, 1) * Matrix4.CreateScale(fTranslateY);
                    //    m = Matrix4.CreateTranslation(0f, 0.5f, 0);
                    //    m = Matrix4.Identity * Matrix4.CreateTranslation(0.5f, 0.5f, 0) * Matrix4.CreateScale(fTranslateY);
                    //    GL.UniformMatrix4(iOurTranslateLocation, true, ref m);
                    //    GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, data.offset);
                    //}


                    //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

                    


                    GL.BindVertexArray(0);
                }

            }


            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
