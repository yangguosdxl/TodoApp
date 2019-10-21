using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestOpenTK
{
    class Mesh : IDisposable
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

            setupMesh();

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

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                GL.DeleteBuffer(VBO);
                GL.DeleteBuffer(EBO);
                GL.DeleteVertexArray(VAO);

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~Mesh()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion
    };
}
