using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine.Components
{

    public class CircleCollider
    {

        public string name = "CircleCollider";

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

        public bool checkColission(CircleCollider other) {

            int c = other.Layer & this.layerMask;
         // Console.WriteLine("Checking colission between: " + this.name + " and " + other.name);
         // Console.WriteLine(Convert.ToString(this.layerMask, toBase: 2));
         // Console.WriteLine(Convert.ToString(other.Layer, toBase: 2));
         // Console.WriteLine(Convert.ToString(c, toBase: 2));
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
