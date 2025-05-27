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

        protected CircleCollider? _collider;
        public CircleCollider? Collider => _collider == null ? null: _collider;

        private event Action<NovaObject>? onDestroy;

        public event Action<NovaObject> OnDestroy
        {
            add { onDestroy += value; }
            remove { onDestroy -= value; }
        }

        public NovaObject(Scene scene)
        {
            _transform = new Transform();
            _containerScene = scene;
            scene.addToObjectPool(this);
        }

        public abstract void Update();


        public abstract void Render();

        public abstract void Clean();

        public void Destroy()
        {
            onDestroy?.Invoke(this);
            _containerScene.RemoveFromObjectPool(this);
            Clean();// this can cause problems with object pooling in the future
        }
        public void CheckColision(CircleCollider? other)
        {
            if (other == null)
                return;
            if (_collider == null)
                return;

            if (_collider.checkColission(other))
            {
                OnColissionStay(other);
            }
        }

        public virtual void OnColissionStay(CircleCollider other)
        {
            // Default implementation does nothing
            // Override this method in derived classes to handle collision events
        }

    }
}
