using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using SDL2;

namespace NovaGame
{
    public class Bullet : NovaObject
    {


        private CircleRenderer circleRenderer;
        private float speed=100f;
        private float size = 10;



        public Bullet(Scene scene, Transform Caster) : base(scene) 
        {
            _transform.Copy(Caster); 
            circleRenderer = new CircleRenderer(_transform, size/2, 0);
            _collider = new CircleCollider(_transform, size / 2, GameManager.playerBulletLayer, GameManager.enemyLayer);
            _collider.name = "bullet";
        }

        public override void Update()
        {
            Transform.MoveUp(speed * Time.DeltaTime);

            float distance = Vector2.Distance(_transform.Position, new Vector2(0,0));
            if (distance>1000) {
                Console.WriteLine("bullet out");
                _containerScene.RemoveFromObjectPool(this);
            }


        }

        public override void Render()
        {
            circleRenderer.Render();
        }

        public override void Clean()
        {
            circleRenderer.Clean();
        }

        public override void OnColissionStay(CircleCollider other)
        {
            // Handle collision with other objects
            // For example, you might want to destroy the bullet or apply damage

            _containerScene.RemoveFromObjectPool(this);
        }
    }
}
