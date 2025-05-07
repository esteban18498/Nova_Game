using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame
{
    public class Bullet : NovaObject
    {

        private CircleRenderer circleRenderer;
        private float speed=100f;
        private float size = 50f;

        public Bullet(Scene scene, Transform Caster) : base(scene) 
        {
            _transform.Copy(Caster); 
            circleRenderer = new CircleRenderer(_transform, size/2, 40);
        }

        public override void Update()
        {
            Transform.MoveUp(speed * Time.DeltaTime);

        }

        public override void Render()
        {
            circleRenderer.Render();
        }

        public override void Clean()
        {
            circleRenderer.Clean();
        }
    }
}
