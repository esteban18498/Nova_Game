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

        protected ICollidable? _collider;
        public ICollidable? Collider => _collider == null ? null: _collider;

        private event Action<NovaObject>? onDestroy;

        public event Action<NovaObject> OnDestroy
        {
            add { onDestroy += value; }
            remove { onDestroy -= value; }
        }

        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { 
                if (value == false) onDeactivate?.Invoke(this);
                if (value == true) onActivate?.Invoke(this);
                isActive = value; 
            }
        }

        private event Action<NovaObject>? onActivate;
        public event Action<NovaObject> OnActivate
        {
            add { onActivate += value; }
            remove { onActivate -= value; }
        }


        private event Action<NovaObject>? onDeactivate;
        public event Action<NovaObject> OnDeactivate
        {
            add { onDeactivate += value; }
            remove { onDeactivate -= value; }
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
        public void CheckColision(ICollidable? other)
        {
            if (other == null)
                return;
            if (_collider == null)
                return;

            if (_collider.CheckCollision(other))
            {
                OnColissionStay(other);
            }
        }

        public virtual void OnColissionStay(ICollidable other)
        {
            // Default implementation does nothing
            // Override this method in derived classes to handle collision events
        }

    }
}
