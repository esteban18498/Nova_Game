using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;
using NovaGame.Engine;

namespace NovaGame
{
    public class Qlock
    {
        private Transform transform;
        private TextRenderer text;
        private float time;


        public Qlock()
        {
            transform = new Transform(NovaEngine.ScreenWidth / 3, NovaEngine.ScreenHeight / 3);
            time = 0;
            text = new TextRenderer(transform, $"0:0");
        }
        public void Update()
        {
            this.time += Time.DeltaTime;

            int seconds = (int)time % 60;
            int minutes = (int)time / 60;


            text.SetMessage($"{minutes}:{seconds}");
        }
        public void Render()
        {
            text.Render();
        }
    }
}
