using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public float requiredEnergy = 100f;
        private float _collectedEnergy = 0f; 
        public float moveSpeed;
        public float size;
        [SerializeField] private Dictionary<Food.Food.Type, int> _consumedAttributes;

        private SpriteRenderer _spriteRenderer;
        private Camera _camera;

        private void CalcSize()
        { 
            var sprite = _spriteRenderer.sprite;
            var localScale = transform.localScale;
            _consumedAttributes = new Dictionary<Food.Food.Type, int>();
            size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _camera = Camera.main;
            CalcSize();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Food.Food food))
            {
                TryEat(food);
            }
        }
        
        private bool TryEat(Food.Food food)
        {
            if (true || food.size < size)
            {
                //transform.localScale += new Vector3(food.eatSizeValue,food.eatSizeValue, 0);
                CalcSize();
                if (_consumedAttributes.ContainsKey(food.type)) 
                {
                    _consumedAttributes[food.type]++;
                }
                else
                {
                    _consumedAttributes[food.type] = 1;
                }
                _collectedEnergy += food.energyValue;
                food.OnEaten();
                return true;
            }
            return false;
        }
    }
}
