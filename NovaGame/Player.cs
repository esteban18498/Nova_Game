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
        private Transform _transform;
        public Transform Transform=>_transform;

        private PlayerController controller;
        private string spritePath = "assets/player.png";
        private SpriteRenderer sprite;
        private AnimationController animationController;

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
            _transform = new Transform();
            controller = new PlayerController(this , _transform);
            sprite = new SpriteRenderer(spritePath, _transform);
            animationController = new AnimationController(sprite, "assets/Animations/PlayerShip/Idle", 4, 0.5f);
        }

        public void Update()
        {
            controller.Update();
            animationController.Update();
        }

        public void Render()
        {
            sprite.Render();
        }

        public void Clean()
        {
            sprite.Clean();
            animationController.Clean();
        }
    }
}
