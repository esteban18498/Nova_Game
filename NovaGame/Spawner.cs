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
        private float spawnInterval = 0.0f; // Spawn every 2 seconds
        private int maxEnemies = 1; // Maximum number of enemies at a time

        private List<Enemy> enemies = new List<Enemy>();

        public EnemySpawner(Scene scene, Player player) : base(scene)
        {
            this.player = player;
            spawnTimer = 0f;
        }
        public override void Update()
        {
            spawnTimer += Time.DeltaTime;
            if (spawnTimer >= spawnInterval && enemies.Count < maxEnemies)
            {
                Enemy newEnemy = new Enemy(ContainerScene, player.Transform);
                enemies.Add(newEnemy);
                newEnemy.OnDestroy += (enemy) =>
                {
                    enemies.Remove(enemy as Enemy);
                };

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
            foreach (Enemy enemy in enemies) {
                _containerScene.RemoveFromObjectPool(enemy);
            }
            enemies.Clear();

            spawnTimer = 0f;
        }
    }
}
