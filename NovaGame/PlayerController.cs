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
            // Check for key presses and update the transform accordingly
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_UP))
            {
                transform.Move( new Vector2(0, 1) * Time.DeltaTime* player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_DOWN))
            {
                transform.Move(new Vector2(0, -1) * Time.DeltaTime * player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_LEFT))
            {
                transform.Move(new Vector2(-1, 0) * Time.DeltaTime * player.Speed);
            }
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_RIGHT))
            {
                transform.Move(new Vector2(1, 0) * Time.DeltaTime * player.Speed);
            }
        }
    }
}
