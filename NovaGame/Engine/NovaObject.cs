using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;

namespace NovaGame.Engine
{
    public abstract class NovaObject
    {
        protected Transform _transform;
        public Transform Transform => _transform;

        protected Scene _containerScene;
        public Scene ContainerScene => _containerScene;

        private CircleCollider? _collider;
        public CircleCollider? Collider => _collider == null ? null: _collider;

        public NovaObject(Scene scene)
        {
            _transform = new Transform();
            _containerScene = scene;
            scene.addToObjectPool(this);
        }

        public abstract void Update();


        public abstract void Render();

        public abstract void Clean();

        public void CheckColision(CircleCollider? other)
        {
            if (other == null)
                return;

            _collider?.checkColission(other);
        }

    }
}
