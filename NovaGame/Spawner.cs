using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine;

namespace NovaGame
{
    public class EnemySpawner : NovaObject
    {
        private Player player;
        private float spawnTimer;
        private float spawnInterval = 2.0f; // Spawn every 2 seconds
        private int maxEnemies = 3; // Maximum number of enemies at a time

        private List<Enemy> activeEnemies = new List<Enemy>();
        private List<Enemy> inactiveEnemies = new List<Enemy>();

        public EnemySpawner(Scene scene, Player player) : base(scene)
        {
            this.player = player;
            spawnTimer = spawnInterval;
        }
        public override void Update()
        {
            spawnTimer += Time.DeltaTime;
            if (spawnTimer >= spawnInterval && activeEnemies.Count < maxEnemies)
            {
                Enemy newEnemy;

                if (inactiveEnemies.Count > 0)
                {
                    newEnemy = inactiveEnemies[0];
                    inactiveEnemies.RemoveAt(0);
                    newEnemy.ResetEnemy();
                }
                else
                {
                    newEnemy = new Enemy(ContainerScene, player.Transform);
                    newEnemy.OnDeactivate += (enemy) =>
                    {
                        activeEnemies.Remove(enemy as Enemy);
                        inactiveEnemies.Add(enemy as Enemy);
                    };
                }


                activeEnemies.Add(newEnemy);


                spawnTimer = 0f;
            }
        }
        public override void Render()
        {
            // No rendering needed for the spawner itself
        }
        public override void Clean()
        {
            // Cleanup logic if necessary
        }

        public void Reset()
        {
            foreach (Enemy enemy in activeEnemies) {
                _containerScene.RemoveFromObjectPool(enemy);
            }
            activeEnemies.Clear();

            spawnTimer = 0f;
        }
    }
}
