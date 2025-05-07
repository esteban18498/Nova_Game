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
    public class Player : NovaObject
    {
       
        private RigidBody rb;


        private PlayerController controller;
        private string spritePath = "assets/player.png";
        private SpriteRenderer sprite;
        private AnimationController animationController;

        private CircleRenderer shield;

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

        public Player(Scene scene) : base(scene) 
        {
            _transform.SetScale(0.5f, 0.5f);
            rb = new RigidBody(_transform);
            controller = new PlayerController(this , _transform, rb);
            sprite = new SpriteRenderer(spritePath, _transform);
            animationController = new AnimationController(sprite, "assets/Animations/PlayerShip/Idle", 4, 0.5f);

            shield = new CircleRenderer(_transform, sprite.Height/2,20f);

        }

        public override void Update()
        {
            controller.Update();
            rb.Update();
            animationController.Update();
        }

        public override void Render()
        {
            sprite.Render();
            shield.Render();
        }

        public override void Clean()
        {
            sprite.Clean();
            animationController.Clean();
        }
    }



}
