using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public GameObject target;
        public float moveSpeed;
        public float energyDrain;
        
        private void Update()
        {
            transform.position = Vector2.Lerp(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}