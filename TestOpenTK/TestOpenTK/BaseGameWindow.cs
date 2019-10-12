using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace TestOpenTK
{
    abstract class BaseGameWindow : GameWindow
    {
        DateTime m_StartTime;

        protected Camera m_Camera = new Camera(new Vector3(0, 0, 2f), new Vector3(0, 0, 1f), new Vector3(0, 1, 0));
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private float sensitivity = (float)(0.05 * Math.PI / 180);
        private bool m_bEnabledRotation = false;

        private float m_fSpeed = 600;

        protected float m_fElpaseSeconds = 0;

        protected Matrix4 m_World2View;
        protected Matrix4 m_View2Proj;

        public BaseGameWindow(int width, int height, string title) :
            base(width, height, GraphicsMode.Default, title)
                { }

        protected override void OnLoad(EventArgs e)
        {
            m_StartTime = DateTime.Now;

            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //GL.ClearColor(1f, 1f, 1f, 1.0f);

            m_View2Proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), Width / Height, 0.1f, 100f);

            m_World2View = m_Camera.LookAt();

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Get the state of the keyboard this frame
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            else if (input.IsKeyDown(Key.Number1))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            else if (input.IsKeyDown(Key.Number2))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else if (input.IsKeyDown(Key.Number3))
            {
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            }
            else if (input.IsKeyDown(Key.Number4))
            {
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            }
            else if (input.IsKeyDown(Key.W))
            {
                Vector3 v = (float)(UpdateTime * -m_fSpeed) * m_Camera.CameraFront;
                m_Camera.CameraPos += v;
            }
            else if (input.IsKeyDown(Key.S))
            {
                Vector3 v = (float)(UpdateTime * m_fSpeed) * m_Camera.CameraFront;
                m_Camera.CameraPos += v;
            }
            else if (input.IsKeyDown(Key.A))
            {
                Vector3 v = (float)(UpdateTime * -m_fSpeed) * m_Camera.CameraRight;
                m_Camera.CameraPos += v;
            }
            else if (input.IsKeyDown(Key.D))
            {
                Vector3 v = (float)(UpdateTime * m_fSpeed) * m_Camera.CameraRight;
                m_Camera.CameraPos += v;
            }

            // Get the mouse state
            var mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                m_bEnabledRotation = true;
                CursorVisible = false;

                if (_firstMove) // this bool variable is initially set to true
                {
                    _lastPos = new Vector2(mouse.X, mouse.Y);
                    _firstMove = false;
                }
                else
                {
                    // Calculate the offset of the mouse position
                    var deltaX = mouse.X - _lastPos.X;
                    var deltaY = mouse.Y - _lastPos.Y;
                    _lastPos = new Vector2(mouse.X, mouse.Y);

                    if (deltaX != 0 || deltaY != 0)
                    {
                        Console.WriteLine($"dx: {deltaX}, dy: {deltaY}, x: {mouse.X}, y: {mouse.Y}, lastpos: {_lastPos}");
                    }

                    

                    //Console.WriteLine($"mouse pos {mouse.X}, {mouse.Y}");

                    // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                    if (deltaX != 0 || deltaY != 0)
                    {
                        
                        m_Camera.Yaw += deltaX * sensitivity;
                        m_Camera.Pitch += deltaY * sensitivity; // reversed since y-coordinates range from bottom to top
                    }

                }
            }
            else
            {
                m_bEnabledRotation = false;
                CursorVisible = true;
                _firstMove = true;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        // In the mouse wheel function we manage all the zooming of the camera
        // this is simply done by changing the FOV of the camera
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            m_Camera.Fov -= e.DeltaPrecise;
            //Console.WriteLine($"mouse wheel: {e.DeltaPrecise}");
            m_View2Proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(m_Camera.Fov), 800.0f / 600, 0.1f, 100f);
            base.OnMouseWheel(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            m_fElpaseSeconds = (float)((DateTime.Now - m_StartTime).TotalMilliseconds / 1000);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Code goes here.

            m_World2View = m_Camera.LookAt();

            DoDraw((float)e.Time);


            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected abstract void DoDraw(float deltaMS);
    }
}
