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
        private Player player;

        public PlayerController(Player player,Transform transform)
        {
            this.transform = transform;
            this.player = player;
        }

        public void Update()
        {

            /*
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
            */

            /*
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_Q))
            {
                transform.Rotate(Time.DeltaTime * player.RotationSpeed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_E))
            {
                transform.Rotate(Time.DeltaTime * -player.RotationSpeed);
            }*/

            // Obtener la posición del mouse
            Vector2 mousePos = NovaEngine.GetMousePosition();
            mousePos = NovaEngine.ScreenToWorld(mousePos);
            Console.WriteLine($"Mouse moved to ({mousePos.X}, {mousePos.Y}), Player pos({transform.Position})");


            // Calcular la dirección hacia el mouse
            Vector2 direction = mousePos - transform.Position;

            // Calcular el ángulo en radianes y convertirlo a grados
            float angle = MathF.Atan2(direction.Y, direction.X)-MathF.PI/2;

            // Actualizar la rotación del transform
            transform.SetRotation(angle);



            if(NovaEngine.IsMouseButtonPressed(NovaEngine.MouseButton.LEFT))
            {
                transform.MoveUp(Time.DeltaTime * player.Speed); ;
            }
        }
    }
}
