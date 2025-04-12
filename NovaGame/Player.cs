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
        private PlayerController controller;
        private string spritePath = "assets/player.png";
        private SpriteRenderer sprite;

        private float _speed=100;
        public float Speed
        {
            get { return _speed; }
            //set { _speed = value; }
        }

        public Player()
        {
            transform = new Transform();
            controller = new PlayerController(this , transform);
            sprite = new SpriteRenderer(spritePath, transform);
        }

        public void Update()
        {
            // Update player position, rotation, etc.
            controller.Update();

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
