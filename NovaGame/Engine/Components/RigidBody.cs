using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine.Components
{
    public class RigidBody
    {
        private Transform transform;
        private Vector2 velocity;
        public Vector2 Velocity => velocity;

        private Vector2 acceleration;
        public Vector2 Acceleration => acceleration;

        private float mass = 1;
        public float Mass => mass;

        public float Drag = 1f;



        public RigidBody(Transform transform)
        {
            this.transform = transform;
            velocity = new Vector2();
        }

        public void SetVelocity(float x, float y)
        {
            velocity = new Vector2(x, y);
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = new Vector2(velocity.X, velocity.Y);
        }

        /*
        public void Update()
        {
            velocity += acceleration * Time.DeltaTime;

            // Update the position based on velocity
            transform.SetPosition(transform.Position.X + velocity.X * Time.DeltaTime, transform.Position.Y + velocity.Y * Time.DeltaTime);



            Vector2 dragForce = velocity * Drag;
            if (velocity.LengthSquared() > 0)
            {


                if (acceleration.X < 0 && dragForce.X < acceleration.X)
                {
                    acceleration.X = 0;
                }
                else if (acceleration.X > 0 && dragForce.X > acceleration.X)
                {
                    acceleration.X = 0;
                }
                else
                {
                    acceleration.X -= dragForce.X;
                }

                if (acceleration.Y < 0 && dragForce.Y < acceleration.Y)
                {
                    acceleration.Y = 0;
                }
                else if (acceleration.Y > 0 && dragForce.Y > acceleration.Y)
                {
                    acceleration.Y = 0;
                }
                else
                {
                    acceleration.Y -= dragForce.Y;
                }

            }
            Debug.WriteLine($"Velocity: {velocity}, Acceleration: {acceleration}");


        }*/

        public void Update()
        {
            // Update velocity based on acceleration
            velocity += acceleration * Time.DeltaTime;

            // Apply drag to velocity
            if (velocity.LengthSquared() > 0) // Avoid division by zero
            {
                Vector2 dragForce = -velocity * Drag * Time.DeltaTime;
                velocity += dragForce;
            }

            // Update the position based on velocity
            transform.SetPosition(
                transform.Position.X + velocity.X * Time.DeltaTime,
                transform.Position.Y + velocity.Y * Time.DeltaTime
            );

            // Reset acceleration for the next frame
            acceleration = Vector2.Zero;
        }


        public void AddLocalForce(Vector2 force)
        {
            float angle = transform.Rotation;

            // Decompose force into local components based on rotation
            float forceX = force.X * MathF.Cos(angle) - force.Y * MathF.Sin(angle);
            float forceY = force.X * MathF.Sin(angle) + force.Y * MathF.Cos(angle);

            // Add the resulting acceleration
            acceleration += new Vector2(forceX, forceY) / mass;
        }
    }
}
