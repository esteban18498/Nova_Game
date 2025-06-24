using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using SDL2;

namespace NovaGame
{
    public class PlayerController
    {
        private Transform transform;
        private RigidBody rb;
        private Player player;
        private IinputController input;

        private float shotTimer=0;

        public PlayerController(Player player, Transform transform, RigidBody rb)
        {
            this.transform = transform;
            this.player = player;
            this.rb = rb;
            shotTimer = player.ShotInterval;
            this.input= new MouseInputController(this.transform);
        }

        public void Update()
        {

            shotTimer += Time.DeltaTime;

            float angle = input.GetRotationAngle(); 
            transform.SetRotation(NovaMath.LerpAngle(transform.Rotation, angle, Time.DeltaTime * player.RotationSpeed));

            if (input.IsFowardPress())
            {
                rb.AddLocalForce(new Vector2(0, 1) * player.Speed);
            }

            if (input.IsShotPress())
            {
                if (shotTimer>=player.ShotInterval)
                {
                    new Bullet(player.ContainerScene, player.Transform);
                    shotTimer = 0;
                }
               
            }

            /*
             * Controlles de Teclado
             *      WASD + QE
             * 
            // Check for key presses and update the transform accordingly
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_UP))
            {
                transform.MoveUp( Time.DeltaTime* player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_DOWN))
            {
                transform.MoveDown(Time.DeltaTime * player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_LEFT))
            {
                transform.MoveLeft(Time.DeltaTime * player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_RIGHT))
            {
                transform.MoveRight(Time.DeltaTime * player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_Q))
            {
                transform.Rotate(Time.DeltaTime * player.RotationSpeed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_E))
            {
                transform.Rotate(Time.DeltaTime * -player.RotationSpeed);
            }*/


            //LoopScreen

            float marginSpace = 50;

            if (transform.Position.Y < - NovaEngine.ScreenHeight/2 - marginSpace)
            {
                transform.SetPosition(transform.Position.X, NovaEngine.ScreenHeight/2 + marginSpace);
            }else 
            if (transform.Position.Y > NovaEngine.ScreenHeight / 2 + marginSpace)
            {
                transform.SetPosition(transform.Position.X, -NovaEngine.ScreenHeight / 2 - marginSpace);
            }

            if (transform.Position.X < -NovaEngine.ScreenWidth / 2 - marginSpace)
            {
                transform.SetPosition( NovaEngine.ScreenWidth / 2 + marginSpace, transform.Position.Y);
            }else 
            if (transform.Position.X > NovaEngine.ScreenWidth / 2 + marginSpace)
            {
                transform.SetPosition( -NovaEngine.ScreenWidth / 2 - marginSpace, transform.Position.Y);
            }
        }
    }

}
