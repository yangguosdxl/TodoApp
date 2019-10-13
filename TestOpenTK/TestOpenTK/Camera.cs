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

        private float m_fPitch;
        private float m_fYaw = 0.5f*(float)Math.PI;
        private float m_Fov = 45;

        public Vector3 CameraPos
        {
            get { return m_CameraPos; }
            set { m_CameraPos = value; }
        }

        public Vector3 CameraRight { get; set; }
        public Vector3 CameraFront { get => m_CameraFront; set => m_CameraFront = value; }
        public float Pitch
        {
            get => m_fPitch;
            set
            {
                m_fPitch = (float)MathHelper.Clamp(value, -89* Math.PI/180, 89 * Math.PI/180);

                //CameraRight = Vector3.Cross(m_CameraFront, m_CameraUp).Normalized();
            }
        }

        public float Yaw
        {
            get => m_fYaw;
            set
            {
                m_fYaw = value;

                //CameraRight = Vector3.Cross(m_CameraFront, m_CameraUp).Normalized();
            }
        }

        public float Fov { get => m_Fov; set => m_Fov = value; }

        public Matrix4 WorldToCameraMatrix { get; set; }

        public Camera(Vector3 cameraPos, Vector3 cameraFront, Vector3 cameraUp)
        {
            m_CameraPos = cameraPos;
            m_CameraFront = cameraFront.Normalized();
            m_CameraUp = cameraUp;

            CameraRight = Vector3.Cross(cameraFront, cameraUp).Normalized();
        }

        public Matrix4 LookAt()
        {
            m_CameraFront.Y = (float)Math.Sin(m_fPitch);
            m_CameraFront.X = (float)(Math.Cos(m_fPitch) * Math.Cos(m_fYaw));
            m_CameraFront.Z = (float)(Math.Cos(m_fPitch) * Math.Sin(m_fYaw));


            Vector3 cameraZ = m_CameraFront;
            Vector3 cameraX = Vector3.Cross(m_CameraUp, cameraZ);
            Vector3 cameraY = Vector3.Cross(cameraZ, cameraX);

            Matrix4 cameraToWorld = Matrix4.Identity;
            cameraToWorld.Row0 = new Vector4(cameraX, 0);
            cameraToWorld.Row1 = new Vector4(cameraY, 0);
            cameraToWorld.Row2 = new Vector4(cameraZ, 0);
            cameraToWorld.Row3 = new Vector4(m_CameraPos, 1);

            Matrix4 worldToCamera;
            Matrix4.Invert(ref cameraToWorld, out worldToCamera);
            WorldToCameraMatrix = worldToCamera;
            //worldToCamera = cameraToWorld;
            return worldToCamera;

            //return Matrix4.LookAt(m_CameraPos, m_CameraPos + m_CameraFront, m_CameraUp);
        }
    }
}
