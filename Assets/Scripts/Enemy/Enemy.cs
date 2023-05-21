using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public GameObject target;
        public GameObject spriteObject;
        public float spriteRotationOffset = 180;
        public float rotationSpeed = 20;
        private float moveSpeed;
        public float minSpeed = 7;
        public float maxSpeed = 9;
        public float energyDrain;


        private void Start()
        {
            moveSpeed = Random.Range(minSpeed, maxSpeed);
        }

        private void Update()
        {
            Vector3 movementDir = target.transform.position - transform.position;
            movementDir.z = 0;

            transform.position += movementDir.normalized * (moveSpeed * Time.deltaTime);

            // rotate enemy to face the movement direction
            float angle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + spriteRotationOffset));
            spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}