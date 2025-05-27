using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame
{
    public class Player : NovaObject, IDamagable
    {
       
        private RigidBody rb;


        private PlayerController controller;
        private string spritePath = "assets/player.png";
        private SpriteRenderer sprite;
        private AnimationController animationController;

        private CircleRenderer shield;
        private Vector4 shieldColor = new Vector4(100, 255, 100, 200);

        #region stats
        private float shotInterval = 0.5f; // 
        private float _speed=500;
        private float _rotationSpeed = 2.5f;
        #endregion

        #region Getters/Setters
        public float Speed
        {
            get { return _speed; }
            //set { _speed = value; }
        }

        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            //set { _rotationSpeed = value; }
        }

        public float ShotInterval
        {
            get { return shotInterval; }
            //set { shotInterval = value; }
        }
        #endregion

        public Player(Scene scene) : base(scene) 
        {
            _transform.SetScale(0.5f, 0.5f);
            rb = new RigidBody(_transform);
            controller = new PlayerController(this , _transform, rb);
            sprite = new SpriteRenderer(spritePath, _transform);
            animationController = new AnimationController(sprite, "assets/Animations/PlayerShip/Idle", 4, 0.5f);

            shield = new CircleRenderer(_transform, sprite.Height* 0.6f, 5, shieldColor);
            _collider = new CircleCollider(this, sprite.Height * 0.6f, GameManager.PLAYER_LAYER, GameManager.ENEMY_LAYER);
            _collider.name = "enemy";
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

        public void TakeDamage(float damage)
        {
            // Implement damage logic here
            Console.WriteLine($"Player took {damage} damage.");
        }
    }



}
