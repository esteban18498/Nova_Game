using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;
using static System.Net.Mime.MediaTypeNames;

namespace NovaGame
{
    public class SurvivalScene
    {
        //private List<Enemy> enemies;
        private Transform sceneTransform;
        private SpriteRenderer background;

        private Player player;
        private TextRenderer text;

        public SurvivalScene() {
            sceneTransform = new Transform();
            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);

            player = new Player();

            //text = new TextRenderer(sceneTransform, "0");

            Init();
        }

        private void Init() 
        {
        }


        public void Update()
        {
            // Update player and other game objects
            player.Update();

        }

        public void Render()
        {
            // Render background and player
            background.Render();
            player.Render();
            //text.Render();
        }
    }
}
