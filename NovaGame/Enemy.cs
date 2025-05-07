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

namespace NovaGame
{
    public class Enemy : NovaObject
    {
        private string spritePath = "assets/enemy.png";
        private SpriteRenderer sprite;
        private Transform target;
        private RigidBody rb;

        private CircleRenderer shield;

        private Vector4 shieldColor = new Vector4(255, 100, 100, 200);

        private float followDistance= 300;


        private float _speed = 100;
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

        public Enemy(Scene scene,Transform Target) : base(scene)
        {

            _transform.SetScale(0.5f, 0.5f);
            target = Target;

            Random rgen= new Random();

            float x = rgen.Next(-NovaEngine.ScreenWidth / 3, NovaEngine.ScreenWidth / 3);
            float y = rgen.Next(-NovaEngine.ScreenHeight / 3, NovaEngine.ScreenHeight / 3);
            //_transform = new Transform(x, y);

            _transform.SetPosition(x,y);
            rb = new RigidBody(_transform);
            sprite = new SpriteRenderer(spritePath, _transform);

            shield = new CircleRenderer(_transform, sprite.Height * 0.6f, 5, shieldColor);

        }

        public override void Update() {

            //follow player
            Vector2 targetPos = target.Position;
            //Vector2 direction = transform.Position - targetPos;
            Vector2 direction = targetPos - _transform.Position;
            float angle = MathF.Atan2(direction.Y, direction.X) + MathF.PI / 2; //-----> + PI/2 = rotar90 grados el sprite. correccion de direccion

            // Actualizar la rotación del transform
            _transform.SetRotation(NovaMath.LerpAngle(_transform.Rotation, angle, Time.DeltaTime * _rotationSpeed));
            
            if( followDistance < Vector2.Distance(_transform.Position, targetPos))
            {
                //transform.MoveDown(Time.DeltaTime * _speed);
                rb.AddLocalForce(new Vector2(0, -1) * _speed);

            }

            rb.Update();
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

    }
}
