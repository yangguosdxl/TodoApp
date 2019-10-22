using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace TestOpenTK
{
    class ModelGameWindow : BaseGameWindow
    {
        Model m_Cube;
        DrawData m_Light = new DrawData();

        int m_VBOSimpleCube = 0;
        Shader m_CubeShader;
        Shader m_LightShader;

        Vector3 m_LightPos;

        public ModelGameWindow(int v1, int v2, string v3) : base(v1, v2, v3)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            //m_CubeShader = new Shader("Cube.vert.glsl", "Cube.frag.glsl");
            m_CubeShader = new Shader("CubeMaterial.vert.glsl", "CubeMaterialMultiLight.frag.glsl");
            m_LightShader = new Shader("Light.vert.glsl", "Light.frag.glsl");

            m_CubeShader.Use();
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            m_Cube = new Model("nanosuit/nanosuit.obj", m_CubeShader);

            //m_Cube.ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45));
            m_Cube.World =
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(0)) *
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)) *
                Matrix4.CreateTranslation(0f, 0f, -3f);

            m_VBOSimpleCube = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBOSimpleCube);
            GL.BufferData(BufferTarget.ArrayBuffer, SimpleModel.SimpleCube.Length * sizeof(float), SimpleModel.SimpleCube, BufferUsageHint.StaticDraw);

            m_Light.VAO = GL.GenVertexArray();
            GL.BindVertexArray(m_Light.VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBOSimpleCube);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            m_Light.shader = m_LightShader;
            m_LightShader.Use();
            GL.EnableVertexAttribArray(0);

            //m_Light.ModelTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45));
            m_LightPos = new Vector3(0f, 0.5f, -2f);

            m_Light.World = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(m_LightPos);
            //m_Light.World = Matrix4.CreateScale(0.2f);
            //m_Light.World = Matrix4.CreateTranslation(m_LightPos);

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
            #region 灯光轨迹
            //float fLightRotateSpeed = 0.7f;
            //float fLightLength = 0.5f;
            //float fLightY = (float)Math.Sin(m_fElpaseSeconds * fLightRotateSpeed) * fLightLength;
            //float fLightX = (float)Math.Cos(m_fElpaseSeconds * fLightRotateSpeed) * fLightLength;
            //m_LightPos.X = fLightX;
            //m_LightPos.Y = fLightY;

            //m_Light.World = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(m_LightPos);

            //Console.WriteLine($"light {m_LightPos}");
            #endregion

            #region Cube轨迹
            float fCubeRotateSpeed = 0.5f;
            m_Cube.World =
                Matrix4.CreateRotationY(m_fElpaseSeconds * fCubeRotateSpeed) *
                //Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) *
                Matrix4.CreateTranslation(0f, -8f, -20f);
            #endregion

            //Vector3 objectColor = new Vector3(1.0f, 0.5f, 0.3f);
            Vector3 lightColor = Vector3.One;

            m_Cube.Shader.Use();

            m_Cube.Shader.SetUniformMat("ModelToWorld", ref m_Cube.World);
            m_Cube.Shader.SetUniformMat("WorldToView", ref m_World2View);
            m_Cube.Shader.SetUniformMat("ViewToProject", ref m_View2Proj);

            // dir light
            m_Cube.Shader.SetUniform3("dirLight.direction", (new Vector4(-0.2f, -1.0f, -0.3f, 0)*m_Camera.WorldToCameraMatrix).Xyz);
            m_Cube.Shader.SetUniform3("dirLight.ambient", 0.05f, 0.05f, 0.05f);
            m_Cube.Shader.SetUniform3("dirLight.diffuse", 0.4f, 0.4f, 0.4f);
            m_Cube.Shader.SetUniform3("dirLight.specular", 0.5f, 0.5f, 0.5f);
            // point light
            for (int i = 0; i < 4; ++i)
            {
                m_Cube.Shader.SetUniform3($"pointLights[{i}].position", (new Vector4(SimpleModel.pointLightPositions[i],1)* m_Camera.WorldToCameraMatrix).Xyz);
                m_Cube.Shader.SetUniform3($"pointLights[{i}].ambient", 0.05f, 0.05f, 0.05f);
                if (i == 2)
                    m_Cube.Shader.SetUniform3($"pointLights[{i}].diffuse", 0f, 1f, 0f);
                else
                    m_Cube.Shader.SetUniform3($"pointLights[{i}].diffuse", 0.8f, 0.8f, 0.8f);
                m_Cube.Shader.SetUniform3($"pointLights[{i}].specular", 1.0f, 1.0f, 1.0f);
                m_Cube.Shader.SetUniform1($"pointLights[{i}].constant", 1.0f);
                m_Cube.Shader.SetUniform1($"pointLights[{i}].linear", 0.09f);
                m_Cube.Shader.SetUniform1($"pointLights[{i}].quadratic", 0.032f);
            }
            // spot light
            m_Cube.Shader.SetUniform3("spotLight.position", (new Vector4(m_Camera.CameraPos,1)* m_Camera.WorldToCameraMatrix).Xyz);
            m_Cube.Shader.SetUniform3("spotLight.direction", (new Vector4(-m_Camera.CameraFront, 1) * m_Camera.WorldToCameraMatrix).Xyz);
            m_Cube.Shader.SetUniform3("spotLight.ambient", 0.0f, 0.0f, 0.0f);
            m_Cube.Shader.SetUniform3("spotLight.diffuse", 1.0f, 1.0f, 1.0f);
            m_Cube.Shader.SetUniform3("spotLight.specular", 1.0f, 1.0f, 1.0f);
            m_Cube.Shader.SetUniform1("spotLight.constant", 1.0f);
            m_Cube.Shader.SetUniform1("spotLight.linear", 0.09f);
            m_Cube.Shader.SetUniform1("spotLight.quadratic", 0.032f);
            m_Cube.Shader.SetUniform1("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(1.5f)));
            m_Cube.Shader.SetUniform1("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(2.5f)));

            m_Cube.Draw();

            // We want to draw all the cubes at their respective positions
            //for (int i = 0; i < SimpleModel.cubePositions.Length; i++)
            //{
            //    // First we create a model from an identity matrix
            //    Matrix4 model = Matrix4.Identity;

            //    float angle = 20.0f * i;
            //    model *= Matrix4.CreateFromAxisAngle(new Vector3(1, 1, 1), angle);
            //    // Then we translate said matrix by the cube position
            //    model *= Matrix4.CreateTranslation(SimpleModel.cubePositions[i] + new Vector3(0,0,-10));
            //    // We then calculate the angle and rotate the model around an axis
                
            //    //model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
            //    // Remember to set the model at last so it can be used by opentk
            //    m_Cube.Shader.SetUniformMat("ModelToWorld", ref model);

            //    m_Cube.Draw();
            //}

            m_Light.shader.Use();

            m_Light.shader.SetUniformMat("ModelToWorld", ref m_Light.World);
            m_Light.shader.SetUniformMat("WorldToView", ref m_World2View);
            m_Light.shader.SetUniformMat("ViewToProject", ref m_View2Proj);

            m_Light.shader.SetUniform3("lightColor", lightColor);

            GL.BindVertexArray(m_Light.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.SimpleCube.Length * sizeof(float));

            for (int i = 0; i < 4; ++i)
            {
                Matrix4 lightWorld = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(SimpleModel.pointLightPositions[i]);
                m_Light.shader.SetUniformMat("ModelToWorld", ref lightWorld);

                GL.BindVertexArray(m_Light.VAO);
                GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.SimpleCube.Length * sizeof(float));
            }
        }
    }
}
