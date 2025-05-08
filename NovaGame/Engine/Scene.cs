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
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].Collider != null)
                {
                    for (int j = i+1; j < pool.Count; j++)
                    {
                        if (pool[j].Collider != null)
                        {
                            if (i != j)
                            {
                                 pool[i].CheckColision(pool[j].Collider);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < pool.Count ;i++)
            {
                pool[i].Update();
            }
        }

        public void Render()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].Render();
            }
        }
    }
}
