using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame
{
    public class PlayerController
    {
        private Transform transform;
        private RigidBody rb;
        private Player player;

        public PlayerController(Player player, Transform transform, RigidBody rb)
        {
            this.transform = transform;
            this.player = player;
            this.rb = rb;
        }

        public void Update()
        {
            /* Controles de Mouse
             *      posision del mouse + LClick para avanzar
             */
            // Obtener la posición del mouse

            Vector2 mousePos = NovaEngine.GetMousePosition();
            mousePos = NovaEngine.ScreenToWorld(mousePos);

            // Calcular la dirección hacia el mouse
            Vector2 direction = mousePos - transform.Position;

            // Calcular el ángulo en radianes y convertirlo a grados
            float angle = MathF.Atan2(direction.Y, direction.X) - MathF.PI / 2;

            // Actualizar la rotación del transform
            transform.SetRotation(NovaMath.LerpAngle(transform.Rotation, angle, Time.DeltaTime * player.RotationSpeed));

            if (NovaEngine.IsMouseButtonPressed(NovaEngine.MouseButton.LEFT))
            {
                //transform.MoveUp(Time.DeltaTime * player.Speed);
                //;

                rb.AddLocalForce(new Vector2(0, 1) * player.Speed);
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

        }
    }

}
