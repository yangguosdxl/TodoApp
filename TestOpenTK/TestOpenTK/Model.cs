using Assimp;
using Assimp.Configs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK
{
    class Model
    {
        public Shader Shader { get; set; }
        public Matrix4 World;
        /*  函数   */
        public Model(string path, Shader shader)
        {
            loadModel(path);
            this.Shader = shader;
        }
        public void Draw()
        {
            foreach (var mesh in meshes)
                mesh.Draw(this.Shader);
        }

        /*  模型数据  */
        List<Mesh> meshes = new List<Mesh>();
        string directory;
        /*  函数   */
        void loadModel(string path)
        {
            AssimpContext importer = new AssimpContext();

            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            Scene scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);

            if (scene == null || (scene.SceneFlags & SceneFlags.Incomplete)!=0 || scene.RootNode == null)
            {
                Console.WriteLine($"ERROR::ASSIMP:: import");
                return;
            }
            directory = Path.GetDirectoryName(path);

            processNode(scene.RootNode, scene);
        }
        void processNode(Node node, Scene scene)
            {
            // 处理节点所有的网格（如果有的话）
            for (int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(processMesh(mesh, scene));
            }
            // 接下来对它的子节点重复这一过程
            for (int i = 0; i < node.ChildCount; i++)
            {
                processNode(node.Children[i], scene);
            }
        }
        Mesh processMesh(Assimp.Mesh mesh, Scene scene)
        {
            List<Vertex> vertices = new List<Vertex>();
            List < uint> indices = new List<uint>();
            List<MeshTexture> textures = new List<MeshTexture>();

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex();
                // 处理顶点位置、法线和纹理坐标
                vertex.pos = mesh.Vertices[i].ToVector3();
                vertex.normal = mesh.Normals[i].ToVector3();
                if (mesh.HasTextureCoords(0))
                {
                    vertex.uv = mesh.TextureCoordinateChannels[0][i].XY().ToVector2();
                }

                vertices.Add(vertex);
            }
            // 处理索引
            foreach (var face in mesh.Faces)
            {
                foreach (uint indice in face.Indices)
                    indices.Add(indice);
            }
            // 处理材质
            if (mesh.MaterialIndex >= 0)
            {
                Material material = scene.Materials[mesh.MaterialIndex];
                MeshTexture[] diffuseMaps = loadMaterialTextures(material,
                                                    TextureType.Diffuse, "material.diffuse");
                textures.AddRange(diffuseMaps);
                MeshTexture[] specularMaps = loadMaterialTextures(material,
                                                    TextureType.Specular, "material.specular");
                textures.AddRange(specularMaps);
            }

            return new Mesh(vertices.ToArray(), indices.ToArray(), textures.ToArray());
        }
        MeshTexture[] loadMaterialTextures(Material mat, TextureType type,
                                             string typeName)
        {
            MeshTexture[] textures = new MeshTexture[mat.GetMaterialTextureCount(type)];
            for (int i = 0; i < mat.GetMaterialTextureCount(type); i++)
            {
                TextureSlot textureSlot;
                mat.GetMaterialTexture(type, i, out textureSlot);

                string path = Path.Combine(directory, textureSlot.FilePath);

                Texture texture = Texture.GetTextureFromCache(path);
                if (texture == null)
                {
                    texture = new Texture(path, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
                    Texture.AddTextureToCache(path, texture);
                }
                MeshTexture meshTexture = new MeshTexture();
                meshTexture.tex = texture;
                meshTexture.shaderTexFieldName = typeName;
                textures[i] = meshTexture;
            }
            return textures;
        }
    };
}
