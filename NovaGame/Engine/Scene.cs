using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;

namespace NovaGame.Engine
{
    public class Scene
    {
        protected Transform sceneTransform;
        private List<NovaObject> pool=new List<NovaObject>();


        public Scene() {
            sceneTransform = new Transform();
        }

        public void addToObjectPool(NovaObject obj)
        {
            pool.Add(obj);
        }

        public void Update()
        {
            foreach (NovaObject obj in pool)
            {
                obj.Update();
            }
        }

        public void Render()
        {
            foreach (NovaObject obj in pool)
            {
                obj.Render();
            }
        }
    }
}
