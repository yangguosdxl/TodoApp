using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace TestOpenTK
{
    class LightGameWindow : BaseGameWindow
    {
        DrawData m_Cube = new DrawData();
        DrawData m_Light = new DrawData();

        int m_VBO = 0;
        Shader m_CubeShader;
        Shader m_LightShader;

        public LightGameWindow(int v1, int v2, string v3) : base(v1, v2, v3)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            m_CubeShader = new Shader("Cube.vert.glsl", "Cube.frag.glsl");
            m_LightShader = new Shader("Light.vert.glsl", "Light.frag.glsl");

            m_VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, SimpleModel.Cube.Length * sizeof(float), SimpleModel.Cube, BufferUsageHint.StaticDraw);

            m_Cube.VAO = GL.GenVertexArray();
            GL.BindVertexArray(m_Cube.VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            m_Cube.shader = m_CubeShader;
            m_CubeShader.Use();
            GL.EnableVertexAttribArray(0);

            m_Cube.ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45));

            m_Light.VAO = GL.GenVertexArray();
            GL.BindVertexArray(m_Light.VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            m_Light.shader = m_LightShader;
            m_LightShader.Use();
            GL.EnableVertexAttribArray(0);

            m_Light.ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45));
            m_Light.World = Matrix4.CreateTranslation(0.7f, 0.7f, 0) * Matrix4.CreateScale(0.2f);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            m_CubeShader.Dispose();
            m_LightShader.Dispose();

            base.OnUnload(e);
        }
        protected override void DoDraw(float deltaMS)
        {
            m_Cube.shader.Use();
            Matrix4 cubeMat = m_Cube.ModelTransform * m_Cube.World * m_World2View * m_View2Proj;
            m_Cube.shader.SetUniformMat("pvm", ref cubeMat);
            GL.BindVertexArray(m_Cube.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.Cube.Length * sizeof(float));

            m_Light.shader.Use();
            Matrix4 lightMat = m_Light.ModelTransform * m_Light.World * m_World2View * m_View2Proj;
            m_Light.shader.SetUniformMat("pvm", ref lightMat);
            GL.BindVertexArray(m_Light.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.Cube.Length * sizeof(float));
        }
    }
}
