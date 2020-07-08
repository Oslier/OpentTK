using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace OpenTK_Project
{
    public class Game : GameWindow
    {
        double time = 0.0f;

        private readonly float[] vertices =
        {
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        private readonly uint[] indices =
        {
            0,1,3, //first triangle
            1,2,3, //second triangle
        };

        Object Object;

        Camera Camera;

        Matrix4 model;
        Matrix4 view;
        Matrix4 projection;

        Vector2 lastPos;
        float dX;
        float dY;
        bool firstMove;

        private int VertexBufferObject;
        private int ElementBufferObject;
        private int VertexArrayObject;

        public Game(int width, int height, string title)
        {
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused) // check to see if the window is focused
            {
                return;
            }

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (keyboard.IsKeyDown(Key.W))
            {
                Camera.MoveForward(e.Time);
            }

            if (keyboard.IsKeyDown(Key.S))
            {
                Camera.Movebackward(e.Time);
            }

            if (keyboard.IsKeyDown(Key.A))
            {
                Camera.MoveLeft(e.Time);
            }

            if (keyboard.IsKeyDown(Key.D))
            {
                Camera.MoveRight(e.Time);
            }

            if (keyboard.IsKeyDown(Key.Space))
            {
                Camera.MoveUp(e.Time);
            }

            if (keyboard.IsKeyDown(Key.LShift))
            {
                Camera.MoveDown(e.Time);
            }

            MouseState mouse = Mouse.GetState();

            if (firstMove)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                dX = mouse.X - lastPos.X;
                dY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);
            }

            Camera.DeltaPitchYaw(dY, dX, e.Time);

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused)  
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            Camera = new Camera();
            CursorVisible = false;
            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);

            Object = new Object();
            Object.Load();

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            time += 8.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Object.Texture.Use(TextureUnit.Texture0);
            Object.Texture2.Use(TextureUnit.Texture1);

            Object.Shader.Use();

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
            Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
            Matrix4 translation = rotation * scale;

            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(0));
            view = Camera.LookForward();

            Object.Shader.SetMatrix4("model", model);
            Object.Shader.SetMatrix4("view", view);
            Object.Shader.SetMatrix4("projection", projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


            Object.Texture.Use(TextureUnit.Texture0);
            Object.Texture2.Use(TextureUnit.Texture1);

            Object.Shader.Use();

            rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
            scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
            translation = rotation * scale;

            model = Matrix4.CreateTranslation(3.0f, 0.5f, 0.5f);
            view = Camera.LookForward();

            Object.Shader.SetMatrix4("model", model);
            Object.Shader.SetMatrix4("view", view);
            Object.Shader.SetMatrix4("projection", projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            Object.Shader.Dispose();
            base.OnUnload(e);
        }
    }
}
