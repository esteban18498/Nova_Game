using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;
using NovaGame.Engine;
using System.Numerics;
using SDL2;
using static System.Net.Mime.MediaTypeNames;

namespace NovaGame
{
    public class Enemy : NovaObject, IDamagable
    {
        #region Components
        private string spritePath = "assets/enemy.png";
        private SpriteRenderer sprite;
        private RigidBody rb;

        private CircleRenderer shield;
        private Vector4 shieldColor = new Vector4(255, 100, 100, 200);
        #endregion

        private Transform target;
        private float shotTimer;

        #region stats
        private float _max_health = 100;
        private float _health;
        private float shotInterval = 2.0f; // Spawn every 2 seconds
        private float followDistance= 300;
        private float _speed = 100;
        private float _rotationSpeed = 2.5f;
        private float precision = MathF.PI / 16; // Precision for aiming PI/16 = 11.25 degrees 
        #endregion

        #region Getters/Setters
        public float Health
        {
            get { return _health; }
            //set { _health = value; }
        }
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
        #endregion

        public Enemy(Scene scene,Transform Target) : base(scene)
        {

            _transform.SetScale(0.5f, 0.5f);
            target = Target;

            rb = new RigidBody(_transform);
            sprite = new SpriteRenderer(spritePath, _transform,  true );

            shield = new CircleRenderer(_transform, sprite.Height * 0.6f, 5, shieldColor);

            _collider = new CircleCollider(this, sprite.Height * 0.6f, GameManager.ENEMY_LAYER, GameManager.PLAYER_LAYER);
            _collider.Name = "enemy";

            _health = _max_health;



            SetSpawnPoint();
        }

        public override void Update() {

            //follow player
            Vector2 targetPos = target.Position;
            //Vector2 direction = transform.Position - targetPos;
            Vector2 direction = targetPos - _transform.Position;
            float angle = MathF.Atan2(direction.Y, direction.X) - MathF.PI / 2; //-----> + PI/2 = rotar90 grados el sprite. correccion de direccion

            // Actualizar la rotación del transform
            _transform.SetRotation(NovaMath.LerpAngle(_transform.Rotation, angle, Time.DeltaTime * _rotationSpeed));
            
            if( followDistance < Vector2.Distance(_transform.Position, targetPos))
            {
                //transform.MoveDown(Time.DeltaTime * _speed);
                rb.AddLocalForce(new Vector2(0, 1) * _speed);

            }
            shotTimer += Time.DeltaTime;

           if (NovaMath.NormalizeAngle(NovaMath.NormalizeAngle(_transform.Rotation) - NovaMath.NormalizeAngle(angle)) <precision) 
            {
                Shot();
            }

            rb.Update();
        }

        private void SetSpawnPoint()
        {
            float x = 0;
            float y = 0;
            Random rgen = new Random();
            int spawnBoundarieTop = (NovaEngine.ScreenHeight + sprite.Height) / 2;
            int spawnBoundarieBottom = (-NovaEngine.ScreenHeight - sprite.Height) / 2;
            int spawnBoundarieLeft = (-NovaEngine.ScreenWidth - sprite.Width) / 2;
            int spawnBoundarieRight = (NovaEngine.ScreenWidth + sprite.Height) / 2;
            switch (rgen.Next(0, 3)) // Randomly choose a spawn side
            {
                case 0://top
                    x = rgen.Next(spawnBoundarieLeft, spawnBoundarieRight);
                    y = spawnBoundarieTop;
                    break;
                case 1://left
                    x = spawnBoundarieLeft;
                    y = rgen.Next(spawnBoundarieBottom, spawnBoundarieTop);
                    break;
                case 2://right
                    x = spawnBoundarieRight;
                    y = rgen.Next(spawnBoundarieBottom, spawnBoundarieTop);
                    break;
                case 3://bottom
                    x = rgen.Next(spawnBoundarieLeft, spawnBoundarieRight);
                    y = spawnBoundarieBottom;
                    break;
            }
            _transform.SetPosition(x, y);
        }

        private void Shot()
        {
            if (shotTimer>= shotInterval)
            {
                Bullet bullet = new Bullet(this.ContainerScene, this.Transform, shieldColor, GameManager.PLAYER_LAYER);
                shotTimer = 0; // Reset the timer
            }
        }

        public override void Render()
        {
            sprite.Render();
            shield.Render();
        }

        public override void Clean() {
            sprite.Clean();
            shield.Clean();
        }


        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                // Handle enemy death
                IsActive = false;
            }
        }

        public void ResetEnemy()
        {
            this.IsActive = true;
            _health = _max_health;
            SetSpawnPoint();
        }
    }
}
