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
            Invoke(nameof(SpawnFoodObject), spawnDelay);
        }

        private void SpawnFoodObject()
        {
            // spawn random food from pool
            int foodIndex = Random.Range(0, foodObjectPool.Length);
            GameObject foodObject =
                Instantiate(foodObjectPool[foodIndex], GetRandomLocInsideBounds(), Quaternion.identity);
            foodObject.GetComponent<Food>().Initialize(this);

            Debug.Log($"Spawned food '{foodObject.name}' at spawner '{gameObject.name}'");
        }

        private Vector2 GetRandomLocInsideBounds()
        {
            Vector2 boxSize = _boxCollider.size / 2;

            Vector2 randOffset = new Vector2(Random.Range(-boxSize.x, boxSize.x), Random.Range(-boxSize.y, boxSize.y));

            return (Vector2) gameObject.transform.localPosition + _boxCollider.offset + randOffset;
        }
    }
}