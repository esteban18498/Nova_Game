using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;
using NovaGame.Engine;
using System.Numerics;

namespace NovaGame
{
    public class Enemy
    {
        private Transform transform;
        private string spritePath = "assets/enemy.png";
        private SpriteRenderer sprite;
        private Transform target;


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

        public Enemy(Transform Target) { 

            target = Target;

            Random rgen= new Random();

            float x = rgen.Next(NovaEngine.ScreenWidth / 3, NovaEngine.ScreenWidth / 2);
            float y = rgen.Next(NovaEngine.ScreenHeight / 3, NovaEngine.ScreenHeight / 2);
            transform = new Transform(x, y);
            sprite = new SpriteRenderer(spritePath, transform); 
       
        }

        public void Update() {

            //follow player
            Vector2 targetPos = target.Position;
            //Vector2 direction = transform.Position - targetPos;
            Vector2 direction = targetPos - transform.Position;
            float angle = MathF.Atan2(direction.Y, direction.X) + MathF.PI / 2; //-----> + PI/2 = rotar90 grados el sprite. correccion de direccion

            // Actualizar la rotación del transform
            transform.SetRotation(NovaMath.LerpAngle(transform.Rotation, angle, Time.DeltaTime * _rotationSpeed));
            transform.MoveDown(Time.DeltaTime * _speed); ;


        }
        public void Render()
        {
            sprite.Render();
        }

        public void Clean() {
            sprite.Clean();
        }

    }
}
