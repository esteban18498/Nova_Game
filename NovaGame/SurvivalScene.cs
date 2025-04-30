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

        private List<NovaObject> pool;

        public SurvivalScene() {
            sceneTransform = new Transform();
            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);
            pool = new List<NovaObject>();

            player = new Player();
            pool.Add(player);
            
            Clock = new Qlock();
            pool.Add(Clock);

            enemy = new Enemy(player.Transform);
            pool.Add(enemy);
            enemy = new Enemy(player.Transform);
            pool.Add(enemy); enemy = new Enemy(player.Transform);
            pool.Add(enemy); enemy = new Enemy(player.Transform);
            pool.Add(enemy); enemy = new Enemy(player.Transform);
            pool.Add(enemy); enemy = new Enemy(player.Transform);
            pool.Add(enemy); enemy = new Enemy(player.Transform);
            pool.Add(enemy); enemy = new Enemy(player.Transform);
            pool.Add(enemy);

            //text = new TextRenderer(sceneTransform, "0");

            Init();
        }

        private void Init() 
        {
        }


        public void Update()
        {
            // Update player and other game objects
            foreach (NovaObject obj in pool)
            {
                obj.Update();
            }

        }

        public void Render()
        {
            // Render background and player
            background.Render();
            foreach (NovaObject obj in pool)
            {
                obj.Render();
            }
        }




       
    }
}
