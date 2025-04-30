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

        public NovaObject()
        {
            _transform = new Transform();
        }

        public abstract void Update();


        public abstract void Render();

        public abstract void Clean();


    }
}
