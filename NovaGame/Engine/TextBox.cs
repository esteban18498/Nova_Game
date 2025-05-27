using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame.Engine
{
    public class TextBox : NovaObject
    {
        TextRenderer textRenderer;

        public TextBox(Scene scene, string message, Vector2 position) : base(scene)
        {
            this._transform.SetPosition(position);
            textRenderer = new TextRenderer(this._transform, message);
        }

        public override void Update()
        {
            //no need
        }

        public override void Render()
        {
            textRenderer.Render();
        }


        public override void Clean()
        {
            textRenderer.Clean();
        }
    }
}
