using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;


namespace TestOpenTK
{
    class Texture
    {
        int m_iTexHandle;
        public Texture(string path)
        {
            Image<Rgb24> image = Image.Load<Rgb24>("container.jpg");

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Get an array of the pixels, in ImageSharp's internal format.
            Rgb24[] tempPixels = image.GetPixelSpan().ToArray();

            m_iTexHandle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, m_iTexHandle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, tempPixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            image.Dispose();
        }

        public void Use(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, m_iTexHandle);
        }
    }
}
