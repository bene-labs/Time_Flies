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
        public float maxSpeed;
        public float minSpeed;
        public float energyDrain;


        private void Start()
        {
            moveSpeed = Random.Range(minSpeed, maxSpeed);
        }

        private void Update()
        {
            Vector3 oldPosition = transform.position;
            Vector3 newPosition = Vector3.Lerp(oldPosition, target.transform.position, moveSpeed * Time.deltaTime);
            newPosition.z = 0f;
            transform.position = newPosition;

            // rotate enemy to face the movement direction
            Vector3 movementDirection = newPosition - oldPosition;
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + spriteRotationOffset));
            spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}