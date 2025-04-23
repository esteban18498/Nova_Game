using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using static System.Net.Mime.MediaTypeNames;

namespace NovaGame
{
    public class SurvivalScene
    {
        //private List<Enemy> enemies;
        private Transform sceneTransform;
        private SpriteRenderer background;

        private Qlock Clock;
        private Player player;
        private Enemy enemy;

        public SurvivalScene() {
            sceneTransform = new Transform();
            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);

            player = new Player();
            Clock = new Qlock();
            enemy = new Enemy(player.Transform);

            //text = new TextRenderer(sceneTransform, "0");

            Init();
        }

        private void Init() 
        {
        }


        public void Update()
        {
            // Update player and other game objects
            Clock.Update();
            player.Update();
            enemy.Update();

        }

        public void Render()
        {
            // Render background and player
            background.Render();
            enemy.Render();
            player.Render();
            Clock.Render();
        }




       
    }
}
