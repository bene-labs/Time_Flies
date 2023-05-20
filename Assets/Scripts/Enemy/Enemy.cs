using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public GameObject target;
        private float moveSpeed;
        public float maxSpeed;
        public float minSpeed;
        public float energyDrain;

        private void Start()
        {
            moveSpeed = Random.Range(minSpeed, maxSpeed);
        }

        private void Update()
        {
            transform.position = Vector2.Lerp(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}