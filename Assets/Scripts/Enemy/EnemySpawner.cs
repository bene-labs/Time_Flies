using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public Vector2 minPos = new Vector2(-500f, -500f);
        public Vector2 maxPos = new Vector2(500f, 500f);

        public float minDelay = 10;
        public float maxDelay = 15;

        public GameObject enemyPrefab;
        public Player.Player player;

        private void Awake()
        {
            player.OnPlayerRespawnStarted += DestroyAllEnemies;
            //player.OnPlayerRespawnFinished += SpawnEnemy;
        }

        private void Start()
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            var spawnPos = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), 0);
            var newEnemy = Instantiate(enemyPrefab, this.transform);
            newEnemy.transform.position = spawnPos;
            newEnemy.GetComponent<Enemy>().target = player.gameObject;
            
            Invoke(nameof(SpawnEnemy), Random.Range(minDelay, maxDelay));
        }

        public void DestroyAllEnemies()
        {
            foreach (var enemy in GetComponentsInChildren<Enemy>())
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}