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
        private EnemySpawner spawner;

        public SurvivalScene() : base()
        {

            background = new SpriteRenderer("assets/Screens/fondo.png", sceneTransform);

            player = new Player(this);
            player.OnDestroy += EndScene;
            Clock = new Qlock(this);
            spawner = new EnemySpawner(this, player);

        }


        public new void Update()
        {
            base.Update();  
        }

        public new void Render() 
        {
            // Render background and scene objects
            background.Render();
            base.Render();
        }

        public void EndScene(NovaObject player) { 
            //GameManager.Instance.ChangeScene(new GameOverScene(player as Player, Clock.TimeElapsed, Clock.Score));

        }
    }
}
