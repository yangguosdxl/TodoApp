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
        int m_VBOSimpleCube = 0;
        Shader m_CubeShader;
        Shader m_LightShader;

        Vector3 m_LightPos;

        Texture m_Container2;
        Texture m_Container2Specular;
        Texture m_EmissionMatrix;

        public LightGameWindow(int v1, int v2, string v3) : base(v1, v2, v3)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            //m_CubeShader = new Shader("Cube.vert.glsl", "Cube.frag.glsl");
            m_CubeShader = new Shader("CubeMaterial.vert.glsl", "CubeMaterial.frag.glsl");
            m_LightShader = new Shader("Light.vert.glsl", "Light.frag.glsl");

            m_VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, SimpleModel.Cube.Length * sizeof(float), SimpleModel.Cube, BufferUsageHint.StaticDraw);

            m_Cube.VAO = GL.GenVertexArray();
            GL.BindVertexArray(m_Cube.VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3*sizeof(float));
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 6 * sizeof(float));
            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 9 * sizeof(float));

            m_Cube.shader = m_CubeShader;
            m_CubeShader.Use();
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);

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

            m_Container2 = new Texture("container2.png", TextureUnit.Texture0);
            m_Container2Specular = new Texture("lighting_maps_specular_color.png", TextureUnit.Texture1);
            m_EmissionMatrix = new Texture("matrix.jpg", TextureUnit.Texture1);
            


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
            float fLightRotateSpeed = 0.7f;
            float fLightLength = 0.5f;
            float fLightY = (float)Math.Sin(m_fElpaseSeconds * fLightRotateSpeed) * fLightLength;
            float fLightX = (float)Math.Cos(m_fElpaseSeconds * fLightRotateSpeed) * fLightLength;
            m_LightPos.X = fLightX;
            m_LightPos.Y = fLightY;

            m_Light.World = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(m_LightPos);

            //Console.WriteLine($"light {m_LightPos}");
            #endregion

            #region Cube轨迹
            float fCubeRotateSpeed = 0.5f;
            m_Cube.World =
                Matrix4.CreateRotationY(m_fElpaseSeconds * fCubeRotateSpeed) *
                //Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) *
                Matrix4.CreateTranslation(0f, 0f, -8f);
            #endregion

            //Vector3 objectColor = new Vector3(1.0f, 0.5f, 0.3f);
            Vector3 lightColor = Vector3.One;

            m_Cube.shader.Use();

            m_Cube.shader.SetUniformMat("ModelToWorld", ref m_Cube.World);
            m_Cube.shader.SetUniformMat("WorldToView", ref m_World2View);
            m_Cube.shader.SetUniformMat("ViewToProject", ref m_View2Proj);

            //m_Cube.shader.SetUniform3("objectColor", ref objectColor);
            //m_Cube.shader.SetUniform3("lightColor", lightColor);
            Vector4 viewLightPos = (new Vector4( m_LightPos, 1) * m_Camera.WorldToCameraMatrix);
            m_Cube.shader.SetUniform4("light.position", viewLightPos);
            Vector3 cameraPos = m_Camera.CameraPos;
            //m_Cube.shader.SetUniform3("viewPos", cameraPos);

            //m_Cube.shader.SetUniform3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
            m_Container2.Use();
            m_Container2Specular.Use(TextureUnit.Texture1);
            m_EmissionMatrix.Use(TextureUnit.Texture2);

            m_Cube.shader.SetUniform1("material.diffuse", 0);
            m_Cube.shader.SetUniform1("material.specular", 1);
            m_Cube.shader.SetUniform1("material.emission", 2);
            m_Cube.shader.SetUniform1("material.shininess", 32.0f);

            m_Cube.shader.SetUniform3("light.ambient", 0.2f, 0.2f, 0.2f);
            m_Cube.shader.SetUniform3("light.diffuse", 0.5f, 0.5f, 0.5f); // 将光照调暗了一些以搭配场景
            m_Cube.shader.SetUniform3("light.specular", 1.0f, 1.0f, 1.0f);

            m_Cube.shader.SetUniform1("light.constant", 1.0f);
            m_Cube.shader.SetUniform1("light.linear", 0.09f);
            m_Cube.shader.SetUniform1("light.quadratic", 0.032f);

            m_Cube.shader.SetUniform3("light.direction", new Vector3(0,0,-1));
            m_Cube.shader.SetUniform1("light.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(5.5f)));
            m_Cube.shader.SetUniform1("light.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(6.5f)));

            GL.BindVertexArray(m_Cube.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.Cube.Length * sizeof(float));

            // We want to draw all the cubes at their respective positions
            for (int i = 0; i < SimpleModel.cubePositions.Length; i++)
            {
                // First we create a model from an identity matrix
                Matrix4 model = Matrix4.Identity;

                float angle = 20.0f * i;
                model *= Matrix4.CreateFromAxisAngle(new Vector3(1, 1, 1), angle);
                // Then we translate said matrix by the cube position
                model *= Matrix4.CreateTranslation(SimpleModel.cubePositions[i] + new Vector3(0,0,-10));
                // We then calculate the angle and rotate the model around an axis
                
                //model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
                // Remember to set the model at last so it can be used by opentk
                m_Cube.shader.SetUniformMat("ModelToWorld", ref model);

                // At last we draw all our cubes
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            m_Light.shader.Use();

            m_Light.shader.SetUniformMat("ModelToWorld", ref m_Light.World);
            m_Light.shader.SetUniformMat("WorldToView", ref m_World2View);
            m_Light.shader.SetUniformMat("ViewToProject", ref m_View2Proj);

            m_Light.shader.SetUniform3("lightColor", lightColor);

            GL.BindVertexArray(m_Light.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, SimpleModel.SimpleCube.Length * sizeof(float));
        }
    }
}
