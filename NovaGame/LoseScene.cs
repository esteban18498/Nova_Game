using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame
{
    public class LoseScene : Scene
    {
        private SpriteRenderer background;

        private TextRenderer gameOverText;

        public LoseScene() : base()
        {
            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);
            gameOverText = new TextRenderer(sceneTransform, "assets/Screens/gameover.png" );
            /*gameOverText.Transform.SetPosition(0, 0);
            gameOverText.Transform.SetScale(0.5f, 0.5f);*/
        }
    }
}
