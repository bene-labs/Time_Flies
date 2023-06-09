using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

namespace Player
{
    [Serializable]
    public struct PlayerTypeSprites
    {
        public PlayerType type;
        public Sprite sprite;
    }

    public enum PlayerType
    {
        Default,
        Mixed,
        Greasy,
        Sweet,
        Spoiled
    }

    [RequireComponent(typeof(Timer))]
    public class Player : MonoBehaviour
    {
        // Exposed Fields -----------------------------------------------------
        [Header("General")] public float requiredEnergy = 100f;
        public float nextRoundEnergyMult = 1.5f;
        public float energyOnSpawn = 10f;
        public float moveSpeed = 8;
        public float mouseDistForMaxSpeed = 5;
        public float rotationSpeed = 20;
        public float size;

        [Header("Animation Timings")] public float eggHatchTime = 3;
        public float invincibilityTime = 0.1f;
        public float damageFlashTime = 0.1f;

        [Header("Player Visuals")] public GameObject spriteObject;
        public SpriteRenderer spriteRenderer;
        public float spriteRotationOffset = -90;
        public Sprite[] playerEggSprites;
        public PlayerTypeSprites[] playerTypeSprites;

        // Events -----------------------------------------------------
        public Action OnPlayerRespawnStarted;
        public Action OnPlayerRespawnFinished;

        // Member Fields -----------------------------------------------------
        private float _collectedEnergy = 0f;

        [SerializeField]
        private Dictionary<Food.Food.Type, int> _consumedAttributes = new Dictionary<Food.Food.Type, int>();

        private bool _isInvincible = false;
        private bool _canGetInput = true;

        private Camera _camera;

        private PlayerType _currentPlayerType;

        private bool _isFirstRound = true;

        // Methods -----------------------------------------------------
        private void CalcSize()
        {
            //var sprite = spriteRenderer.sprite;
            //var localScale = transform.localScale;
            //size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
        }

        // Start is called before the first frame update
        void Awake()
        {
            _camera = Camera.main;

            CalcSize();
        }

        private void Start()
        {
            _isFirstRound = true;
            Respawn();
        }

        // Update is called once per frame
        void Update()
        {
            if (_canGetInput && Input.GetMouseButton(0))
            {
                // move player
                Vector3 movementDir = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                movementDir.z = 0;

                // calculate a multiplier for the speed based on the distance the mouse has to the player.
                // if its closer than the threshold the player's speed is reduced.
                float mouseDistMultiplier = Math.Clamp(movementDir.magnitude / mouseDistForMaxSpeed, 0, 1);

                transform.position += movementDir.normalized * (mouseDistMultiplier * (moveSpeed * Time.deltaTime));


                // rotate player to face the movement direction
                float angle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + spriteRotationOffset));
                spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation,
                    rotationSpeed * Time.deltaTime);
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
            var defaultColor = spriteRenderer.color;

            DOVirtual.Color(Color.red, defaultColor, damageFlashTime, value => spriteRenderer.color = value)
                .SetEase(Ease.OutCirc);

            yield return new WaitForSeconds(invincibilityTime);
            _isInvincible = false;
            // Add Inviciblity Frames and Red flash animation here

            if (_collectedEnergy <= 0)
                OnDeath();
        }

        private void OnDeath()
        {
            UIManager.SetDeathScreenVisibility(true);
        }

        public void OnRoundEnded()
        {
            _isFirstRound = false;

            if (_collectedEnergy >= requiredEnergy)
            {
                requiredEnergy *= nextRoundEnergyMult;
                Respawn();
            }
            else
            {
                OnDeath();
            }
        }

        private void Respawn()
        {
            OnPlayerRespawnStarted?.Invoke();

            if (!_isFirstRound)
            {
                _currentPlayerType = EvalPlayerTypeBasedOnConsumedFood();
                StartCoroutine(RespawnCoroutine());
            }
            else OnRespawnFinished();

            _collectedEnergy = energyOnSpawn;
            _consumedAttributes = new Dictionary<Food.Food.Type, int>();

            UpdateEnergyBar();
        }

        private void OnRespawnFinished()
        {
            GetComponent<Timer>().RestartTimer();
            OnPlayerRespawnFinished?.Invoke();
        }

        private PlayerType EvalPlayerTypeBasedOnConsumedFood()
        {
            // if no food has been eaten we will display the default fly sprite
            int mostFoodCount = 0;
            PlayerType playerType = PlayerType.Default;

            // check all eaten food and display the sprite for the most eaten type
            // if two or more food types have been eaten the same "most" amount of times we display the "mixed fly" sprite 
            foreach (var consumedFood in _consumedAttributes)
            {
                if (consumedFood.Value >= mostFoodCount)
                {
                    playerType = consumedFood.Value == mostFoodCount
                        ? PlayerType.Mixed
                        : FoodToPlayerType(consumedFood.Key);

                    mostFoodCount = consumedFood.Value;
                }
            }

            return playerType;
        }

        private PlayerType FoodToPlayerType(Food.Food.Type foodType)
        {
            switch (foodType)
            {
                case Food.Food.Type.Greasy: return PlayerType.Greasy;
                case Food.Food.Type.Sweet: return PlayerType.Sweet;
                case Food.Food.Type.Spoiled: return PlayerType.Spoiled;
                default: return PlayerType.Default;
            }
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

            Debug.Log($"Food consumed - Type: {food.type}, Amount {_consumedAttributes[food.type]}");

            _collectedEnergy += food.energyValue;
            UpdateEnergyBar();
            food.OnEaten();
            return true;
        }

        private void UpdateEnergyBar()
        {
            UIManager.SetEnergyBar(_collectedEnergy, requiredEnergy);
        }

        IEnumerator RespawnCoroutine()
        {
            _canGetInput = false;
            _isInvincible = true;

            float eggStateDisplayTime = eggHatchTime / playerEggSprites.Length;

            foreach (Sprite eggSprite in playerEggSprites)
            {
                spriteRenderer.sprite = eggSprite;
                yield return new WaitForSeconds(eggStateDisplayTime);
            }

            // find correct sprite for the new player type and display it
            foreach (PlayerTypeSprites playerTypeSprite in playerTypeSprites)
            {
                if (playerTypeSprite.type == _currentPlayerType)
                {
                    spriteRenderer.sprite = playerTypeSprite.sprite;
                }
            }

            _canGetInput = true;
            _isInvincible = false;

            OnRespawnFinished();
        }
    }
}