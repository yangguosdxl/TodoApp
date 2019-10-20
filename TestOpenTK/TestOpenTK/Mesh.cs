using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestOpenTK
{
    class Mesh
    {
        /*  渲染数据  */
        int VAO, VBO, EBO;

        /*  网格数据  */
        public Vertex[] vertices;
        public uint[] indices;
        public MeshTexture[] textures;
        /*  函数  */
        public Mesh(Vertex[] vertices, uint[] indices, MeshTexture[] textures)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;

        }
        public void Draw(Shader shader)
        {
            shader.Use();
            for(int i = 0; i < this.textures.Length; ++i)
            {
                MeshTexture meshTex = this.textures[i];
                meshTex.tex.Use(TextureUnit.Texture0 + i);
                shader.SetUniform1(meshTex.shaderTexFieldName, i);
            }

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }


        /*  函数  */
        void setupMesh()
        {
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * Marshal.SizeOf(typeof(Vertex)), this.vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }
    };
}
