using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Project
{
    class Camera
    {
        public Vector3 cameraPosition;
        public Vector3 cameraTarget;
        public Vector3 cameraDirection;

        Vector3 cameraRight;
        Vector3 cameraUp;

        float speed = 1.5f;
        float sensitivity = 10.0f;

        Vector3 front;
        Vector3 up;

        float pitch;
        float yaw;

        public Camera()
        {
            cameraPosition = new Vector3(0.0f, 0.0f, -3.0f);

            cameraTarget = Vector3.Zero;
            cameraDirection = Vector3.Normalize(cameraPosition - cameraTarget);

            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = Vector3.UnitY;
            cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));

            cameraUp = Vector3.Cross(cameraDirection, cameraRight);
        }

        public Camera(Vector3 position, Vector3 target)
        {
            cameraPosition = position;

            cameraTarget = target;
            cameraDirection = Vector3.Normalize(cameraPosition - cameraTarget);

            up = Vector3.UnitY;
            cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
        }

        public Matrix4 LookAt(Vector3 position)
        {
            return Matrix4.LookAt(cameraPosition, position, Vector3.UnitY);
        }

        public Matrix4 LookForward()
        {
            return Matrix4.LookAt(cameraPosition, cameraPosition + front, Vector3.UnitY);
        }

        public void MoveForward(double deltaTime)
        {
            cameraPosition += front * speed * (float)deltaTime;
        }
        public void Movebackward(double deltaTime)
        {
            cameraPosition -= front * speed * (float)deltaTime;
        }
        public void MoveUp(double deltaTime)
        {
            cameraPosition += up * speed * (float)deltaTime;
        }
        public void MoveDown(double deltaTime)
        {
            cameraPosition -= up * speed * (float)deltaTime;
        }
        public void MoveRight(double deltaTime)
        {
            cameraPosition += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)deltaTime;
        }
        public void MoveLeft(double deltaTime)
        {
            cameraPosition -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)deltaTime;
        }

        public void DeltaPitchYaw(float DeltaPitch, float DeltaYaw, double dealtaTime)
        {
            pitch -= DeltaPitch * sensitivity * (float)dealtaTime;
            yaw += DeltaYaw * sensitivity * (float)dealtaTime;

            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            else if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
        }
    }
}
