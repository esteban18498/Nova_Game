using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;

namespace NovaGame
{
    public class LoseScene : Scene
    {
        private SpriteRenderer background;

        private TextBox gameOverText;
        private TextBox scoreText;

        private TextBox resetText;

        public LoseScene() : base()
        {
            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);

            Vector2 TitlePosition = new Vector2(0, NovaEngine.ScreenHeight / 3);
            gameOverText = new TextBox(this, "You Lose", TitlePosition);

            Vector2 scorePosition = new Vector2(0,0);
            scoreText = new TextBox(this, "Score:", scorePosition);


            Vector2 resetTextPos = new Vector2(0, -NovaEngine.ScreenHeight / 3);
            scoreText = new TextBox(this, "Space to reset", resetTextPos);

        }

        public new void Update()
        {
            base.Update();
            if (NovaEngine.IsKeyPressed(NovaEngine.KEY_ESP))
            {
                
                GameManager.Instance.ChangeGameStatus(gameStatus.game);
            }
        }
    }
}
