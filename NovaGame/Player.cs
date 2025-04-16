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

        private float _speed=500;
        public float Speed
        {
            get { return _speed; }
            //set { _speed = value; }
        }

        private float _rotationSpeed = 2.5f;
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            //set { _rotationSpeed = value; }
        }

        public Player()
        {
            transform = new Transform();
            controller = new PlayerController(this , transform);
            sprite = new SpriteRenderer(spritePath, transform);
        }

        public void Update()
        {
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
