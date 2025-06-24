using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine.Components
{

    public interface ICollidable
    {
        bool CheckCollision(ICollidable? other);
        byte Layer { get; }
        byte LayerMask { get; }

        NovaObject Owner { get; }

        string Name { get; set; }
    }


    public class CircleCollider : ICollidable
    {

        private string name = "CircleCollider";

        public string Name
        {
            get => name;
            set => name = value;
        }

        private NovaObject owner;
        public NovaObject Owner => owner;

        private Transform transform;
        public Transform Transform => transform; 
        
        private float radius;
        public float Radius => radius;


        // 8 layers of collitions
        // colliders lives in layers and check collition aginst layer masks
        private byte layer;
        public byte Layer => layer;

        private byte layerMask;
        public byte LayerMask => layerMask;



        public CircleCollider(NovaObject owner, float radius, byte layer =0b00000000, byte layerMask = 0b00000000)
        {
            this.owner = owner;
            this.transform = owner.Transform;
            this.radius = radius;
            this.layer = layer;
            this.layerMask = layerMask;
        }

        public bool CheckCollision(ICollidable? Iother) {

            CircleCollider other = Iother as CircleCollider;

            int c = other.Layer & this.layerMask;

            if ((other.Layer & this.layerMask) ==0)
            {
                return false;
            }


            float distance = Vector2.Distance(transform.Position, other.transform.Position);
            if (distance < radius+other.Radius) {

                return true;            
            }

            return false;
        }
    }
}
