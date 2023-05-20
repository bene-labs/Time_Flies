using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float damageFlashTime;
        [SerializeField] private float invincibilityTime;
        
        [Header("General")]
        public float requiredEnergy = 100f;
        public float moveSpeed;
        public float size;

        [Header("Player Visuals")] 
        public Dictionary<Food.Food.Type, Sprite> playerTypeSprites;
        public  Sprite playerEggSprite;
        
        [Header("UI")]
        public TextMeshProUGUI energyText;
        public GameObject deathScreen;

        private float _collectedEnergy = 0f;
        
        [SerializeField] private Dictionary<Food.Food.Type, int> _consumedAttributes;
        private bool _isInvincible = false;
        private bool _canGetInput = true;
        
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;

        private Food.Food.Type _currentPlayerType;
            
        private void CalcSize()
        {
            //var sprite = _spriteRenderer.sprite;
            //var localScale = transform.localScale;
            _consumedAttributes = new Dictionary<Food.Food.Type, int>();
            //size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
        }
        
        // Start is called before the first frame update
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _camera = Camera.main;
            
            ResetPlayer();
            
            UpdateEnergyBar();
            CalcSize();
        }

        // Update is called once per frame
        void Update()
        {
            if (_canGetInput && Input.GetMouseButton(0))
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
            else if (collision.gameObject.TryGetComponent(out Enemy.Enemy enemy))
            {
                TakeDamage(enemy.energyDrain);
            }
        }

        private void TakeDamage(float damage)
        {
            if (_isInvincible)
                return;
            _collectedEnergy -= damage;
            StartCoroutine(DamageAnimation(damage));
        }

        IEnumerator DamageAnimation(float damage)
        {
            _isInvincible = true;
            
            if (_collectedEnergy < 0)
                _collectedEnergy = 0;
            UpdateEnergyBar();
            var defaultColor = _spriteRenderer.color;

            DOVirtual.Color(Color.red, defaultColor, damageFlashTime, value => _spriteRenderer.color = value)
                .SetEase(Ease.OutCirc);

            yield return new WaitForSeconds(invincibilityTime);
            _isInvincible = false;
            // Add Inviciblity Frames and Red flash animation here

            if (_collectedEnergy <= 0)
                OnDeath();
        }

        private void OnDeath()
        {
            //deathScreen.SetActive(true);
        }

        private void ResetPlayer()
        {
            //_currentPlayerType = EvalMostConsumedFood();
            _collectedEnergy = 0;

            StartCoroutine(RespawnPlayer());
        }

        private Food.Food.Type EvalMostConsumedFood()
        {
            int mostFoodCount = 0;
            Food.Food.Type mostFoodType = Food.Food.Type.Mixed;

            foreach (var consumedFood in _consumedAttributes)
            {
                if (consumedFood.Value > mostFoodCount)
                {
                    mostFoodCount = consumedFood.Value;
                    mostFoodType = consumedFood.Key;
                }

                if (consumedFood.Value == mostFoodCount)
                {
                    mostFoodCount = consumedFood.Value;
                    mostFoodType = Food.Food.Type.Mixed;
                }
            }

            return mostFoodType;
        }

        private bool TryEat(Food.Food food)
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
            UpdateEnergyBar();
            food.OnEaten();
            return true;
        }

        private void UpdateEnergyBar()
        {
            if (energyText)
            {
                energyText.text = _collectedEnergy + " / " + requiredEnergy;
            }
        }

        IEnumerator RespawnPlayer()
        {
            _canGetInput = false;
            _isInvincible = true;
            //_spriteRenderer.sprite = playerEggSprite;
            
            yield return new WaitForSeconds(5);

            //_spriteRenderer.sprite = playerTypeSprites[_currentPlayerType];
            _canGetInput = true;
            _isInvincible = false;
        }
    }
}
