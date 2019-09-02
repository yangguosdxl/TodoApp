using OpenTK;
using OpenTK.Graphics.OpenGL4;

using System;


namespace TestOpenTK
{
    class Camera
    {
        Vector3 m_CameraPos;
        Vector3 m_CameraFront;
        Vector3 m_CameraUp;

        public Vector3 CameraPos
        {
            get { return m_CameraPos; }
            set { m_CameraPos = value; }
        }

        public Vector3 CameraRight { get; set; }
        public Vector3 CameraFront { get => m_CameraFront; set => m_CameraFront = value; }

        public Camera(Vector3 cameraPos, Vector3 cameraFront, Vector3 cameraUp)
        {
            m_CameraPos = cameraPos;
            CameraFront = cameraFront.Normalized();
            m_CameraUp = cameraUp;

            CameraRight = Vector3.Cross(cameraFront, cameraUp).Normalized();
        }

        public Matrix4 LookAt()
        {
            return Matrix4.LookAt(m_CameraPos, m_CameraPos + CameraFront, m_CameraUp);
        }
    }
}
