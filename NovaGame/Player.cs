using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame
{
    public class Player
    {
        private Transform transform;
        private string spritePath = "assets/player.png";
        private SpriteRenderer sprite;

        public Player()
        {
            transform = new Transform();
            sprite = new SpriteRenderer(spritePath, transform);
        }

        public void Update()
        {
            // Update player position, rotation, etc.
            transform.Rotate(1 * Time.DeltaTime);
        }

        public void Render()
        {
            sprite.Render();
        }

        public void Clean()
        {
            sprite.Clean();
        }
    }
}
