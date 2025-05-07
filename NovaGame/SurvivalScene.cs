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
    public class SurvivalScene : Scene
    {
        //private List<Enemy> enemies;

        private SpriteRenderer background;

        private Qlock Clock;
        private Player player;

        public SurvivalScene() : base()
        {

            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);

            player = new Player(this);
            Clock = new Qlock(this);

            new Enemy(this, player.Transform);
            new Enemy(this, player.Transform);
            new Enemy(this, player.Transform);
            new Enemy(this, player.Transform);
            new Enemy(this, player.Transform);
            new Enemy(this, player.Transform);
            new Enemy(this, player.Transform);
        }


        public new void Update()
        {
            base.Update();
        }

        public new void Render() 
        {
            // Render background and player
            background.Render();
            base.Render();
        }
       
    }
}
