using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine.Components
{
    public class Transform
    {
        private Vector2 position;

        public Vector2 Position => position;

        private Vector2 scale;

        public Vector2 Scale => scale;

        private float rotation;
        public float Rotation => rotation;


        public Transform()
        {
            position = new Vector2();
            scale = new Vector2(1,1);
            rotation = 0;
        }

        public Transform(float x, float y)
        {
            position = new Vector2(x, y);
            scale = new Vector2(1, 1);
            rotation = 0;
        }

        public Transform(Vector2 position)
        {
            this.position = new Vector2(position.X, position.Y);
            scale = new Vector2(1, 1);
            rotation = 0;
        }
        public Transform(float x, float y, float r)
        {
            position = new Vector2(x, y);
            scale = new Vector2(1, 1);
            rotation = r;
        }

        public Transform(Vector2 position, float r)
        {
            this.position = new Vector2(position.X, position.Y);
            scale = new Vector2(1, 1);
            rotation = r;
        }

        public void Copy(Transform transformToCopy)
        {
            position = transformToCopy.position;
            scale = transformToCopy.scale;
            rotation = transformToCopy.rotation;    
        }

        public void SetScale(float x, float y)
        {
            scale = new Vector2(x, y);
        }
        public void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public void Rotate(float rotation)
        {
            this.rotation += rotation;
        }

        public void SetPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = new Vector2(position.X, position.Y);
        }

        public void Move(float x, float y)
        {
            position += new Vector2(x, y);
        }
        public void Move(Vector2 position)
        {
            this.position += new Vector2(position.X, position.Y);
        }

        public void MoveUp(float distance)
        {
            float deltaX = -MathF.Sin(rotation) * distance;
            float deltaY = MathF.Cos(rotation) * distance;
            position += new Vector2(deltaX, deltaY);
        }

        public void MoveDown(float distance)
        {  
            float deltaX = MathF.Sin(rotation) * distance;
            float deltaY = -MathF.Cos(rotation) * distance;

            position += new Vector2(deltaX, deltaY);
        }

        public void MoveRight(float distance)
        {
            float deltaX = MathF.Cos(rotation) * distance;
            float deltaY = MathF.Sin(rotation) * distance;

            position += new Vector2(deltaX, deltaY);
        }

        public void MoveLeft(float distance) 
        {   
                float deltaX = -MathF.Cos(rotation) * distance;
                float deltaY = -MathF.Sin(rotation) * distance;
                position += new Vector2(deltaX, deltaY);
        }
    }
}
