using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Food
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FoodSpawner : MonoBehaviour
    {
        public float minRespawnDelay = 10;
        public float maxRespawnDelay = 20;

        public GameObject[] foodObjectPool;

        private BoxCollider2D _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            SpawnFoodObject();
        }

        public void OnFoodEaten()
        {
            float spawnDelay = Random.Range(minRespawnDelay, maxRespawnDelay);
            Debug.Log($"New food spawned by '{gameObject.name}' in {spawnDelay} seconds");
            StartCoroutine(SpawnFoodAfterDelay(spawnDelay));
        }

        private void SpawnFoodObject()
        {
            // spawn random food from pool
            int foodIndex = Random.Range(0, foodObjectPool.Length);
            GameObject foodObject = Instantiate(foodObjectPool[foodIndex],GetRandomLocInsideBounds(),Quaternion.identity);
            foodObject.GetComponent<Food>().Initialize(this);
            
            Debug.Log($"Spawned food '{foodObject.name}' at spawner '{gameObject.name}'");
        }

        private Vector2 GetRandomLocInsideBounds()
        {
            Bounds spawnerBounds = _boxCollider.bounds;

            Vector2 randOffset = new Vector2(
                Random.Range(spawnerBounds.min.x, spawnerBounds.max.x),
                Random.Range(spawnerBounds.min.y, spawnerBounds.max.y));

            return (Vector2)gameObject.transform.position;// + _boxCollider.offset + randOffset;
        }

        IEnumerator SpawnFoodAfterDelay(float spawnDelay)
        {
            yield return new WaitForSeconds(spawnDelay);

            SpawnFoodObject();
        }
    }
}