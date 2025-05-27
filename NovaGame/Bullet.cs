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
        private Vector4 bulletColor = new Vector4(255, 255, 255, 255);

        private float _damage = 100f;
        private float _speed=300f;
        private float _size = 10;

        private Byte _collideWith;

        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /**
public float Size
{
    get { return _size; }
    set { _size = value; }
}


public Byte CollideWith
{
    get { return _collideWith; }
    set { _collideWith = value; }
}
public Vector4 BulletColor
{
    get { return bulletColor; }
    set { bulletColor = value; }
}*/


        public Bullet(Scene scene, Transform Caster, Vector4? Color=null, Byte CollideWith= GameManager.ENEMY_LAYER) : base(scene) 
        {
            _transform.Copy(Caster);

            if ( Color != null)
            { 
                bulletColor = (Vector4)Color;
                circleRenderer = new CircleRenderer(_transform, _size/2, 0, Color);
            }
            else
            {
                circleRenderer = new CircleRenderer(_transform, _size / 2, 0, bulletColor);
            }

            _collideWith = CollideWith;
            _collider = new CircleCollider(this, _size / 2, GameManager.BULLET_LAYER, _collideWith);
            _collider.name = "bullet";
        }

        public override void Update()
        {
            Transform.MoveUp(_speed * Time.DeltaTime);

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

            if (other.Owner is IDamagable)
            {
                IDamagable target = (IDamagable)other.Owner;
                target.TakeDamage(_damage);
                Destroy();
            }
        }
    }
}
