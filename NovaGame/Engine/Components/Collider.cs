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
        private Transform transform;
        public Transform Transform => transform; 
        
        private float radius;
        public float Radius => radius;

        CircleCollider(Transform transform, float radius)
        {
            this.transform = transform;
            this.radius = radius;
        }

        public bool checkColission(CircleCollider other) {

            float distance = Vector2.Distance(transform.Position, other.transform.Position);
            if (distance < radius+other.Radius) {

                return true;            
            }

            return false;
        }
    }
}
