using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

public class Shader : IDisposable
{
    int Handle;
    string m_VertexPath;
    string m_FragPath;

    public Shader(string vertexPath, string fragmentPath)
    {
        m_VertexPath = vertexPath;
        m_FragPath = fragmentPath;

        string VertexShaderSource;

        using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
        {
            VertexShaderSource = reader.ReadToEnd();
        }

        string FragmentShaderSource;

        using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
        {
            FragmentShaderSource = reader.ReadToEnd();
        }

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);

        GL.CompileShader(VertexShader);

        string infoLogVert = GL.GetShaderInfoLog(VertexShader);
        if (infoLogVert != System.String.Empty)
            System.Console.WriteLine($"{vertexPath}:{infoLogVert}");

        GL.CompileShader(FragmentShader);

        string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

        if (infoLogFrag != System.String.Empty)
            System.Console.WriteLine($"{fragmentPath}:{infoLogFrag}");

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string name)
    {
        int iLocation = GL.GetAttribLocation(Handle, name);
        if (iLocation == -1)
        {
            Console.WriteLine($"{m_VertexPath},{m_FragPath}: Can not find attrib name {name}");
        }
        return iLocation;
    }

    public int GetUniformLocation(string name)
    {
        int iLocation = GL.GetUniformLocation(Handle, name);
        if (iLocation == -1)
        {
            Console.WriteLine($"{m_VertexPath},{m_FragPath}: Can not find uniform name {name}");
        }
        return iLocation;
    }

    public void SetUniformMat(string name, ref Matrix4 mat)
    {
        int location = GetUniformLocation(name);
        if (location == -1) return;

        GL.UniformMatrix4(location, false, ref mat);
    }

    public void SetUniform3(string name, ref Vector3 v3)
    {
        int location = GetUniformLocation(name);
        if (location == -1) return;

        GL.Uniform3(location, ref v3);
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

            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
    ~Shader()
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
}