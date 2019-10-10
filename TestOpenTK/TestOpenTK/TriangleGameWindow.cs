using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
 
namespace TestOpenTK
{
    class TriangleGameWindow : BaseGameWindow
    {
        DrawData m_Triangle = new DrawData();

        int m_VBO = 0;
        Shader m_TriangleShader;

        public TriangleGameWindow(int v1, int v2, string v3) : base(v1, v2, v3)
        { 
        }

        protected override void OnLoad(EventArgs e)
        {
            Console.WriteLine("triangle on load .......................");
            m_TriangleShader = new Shader("Cube.vert.glsl", "Cube.frag.glsl");

            m_VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, SimpleModel.Triangle.Length * sizeof(float), SimpleModel.Triangle, BufferUsageHint.StaticDraw);

            m_Triangle.VAO = GL.GenVertexArray();
            GL.BindVertexArray(m_Triangle.VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            m_Triangle.shader = m_TriangleShader;
            m_TriangleShader.Use();
            GL.EnableVertexAttribArray(0);

            //m_Cube.ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45));
            m_Triangle.World = Matrix4.CreateTranslation(0f, 0f, -5f);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            m_TriangleShader.Dispose();

            base.OnUnload(e);
        }
        protected override void DoDraw(float deltaMS)
        {
            m_Triangle.shader.Use();


            m_Triangle.shader.SetUniformMat("ModelToWorld", ref m_Triangle.World);
            m_Triangle.shader.SetUniformMat("WorldToView", ref m_World2View);
            m_Triangle.shader.SetUniformMat("ViewToProject", ref m_View2Proj);
            GL.BindVertexArray(m_Triangle.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.Triangle.Length * sizeof(float));
        }
    }
}
