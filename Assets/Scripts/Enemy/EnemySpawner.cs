using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 minPos = new Vector2(-500f, -500f);
    public Vector2 maxPos = new Vector2(500f, 500f);

    public float minDelay = 5;
    public float maxDelay = 10;

    public GameObject enemyPrefab;
    public GameObject player;
    private void Start()
    {
        Invoke("SpawnEnemy", Random.Range(minDelay, maxDelay));
    }

    void SpawnEnemy()
    {
        var spawnPos = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), 0);
        var newEnemy = Instantiate(enemyPrefab, this.transform);
        newEnemy.transform.position = spawnPos;
        newEnemy.GetComponent<Enemy.Enemy>().target = player;
    }

    public void DestroyAll()
    {
        foreach (var enemy in GetComponentsInChildren<Enemy.Enemy>())
        {
            Destroy(enemy);
        }
    }
}
