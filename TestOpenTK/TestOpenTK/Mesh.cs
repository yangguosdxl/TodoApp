using System;
using System.Collections.Generic;


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
        public List<Vertex> vertices;
        public List<uint> indices;
        public List<Texture> textures;
        /*  函数  */
        public Mesh(List<Vertex> vertices, List<uint> indices, List<Texture> textures)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;

            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        }
        public void Draw(Shader shader)
        {

        }


        /*  函数  */
        void setupMesh()
        {

        }
    };
}
