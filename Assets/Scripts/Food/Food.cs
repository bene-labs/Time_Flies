using UnityEngine;

namespace Food
{
    public class Food : MonoBehaviour
    {
        public enum Type
        {
            Red,
            Blue,
            Green
        }

        public Type type;
        public float size;

        public float energyValue;
        //public float eatSizeValue;

        private SpriteRenderer _spriteRenderer;
        private FoodSpawner _parentSpawner;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var sprite = _spriteRenderer.sprite;
            var localScale = transform.localScale;
            size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;

            switch (type)
            {
                case Type.Red:
                    _spriteRenderer.color = Color.red;
                    break;
                case Type.Green:
                    _spriteRenderer.color = Color.green;
                    break;
                case Type.Blue:
                    _spriteRenderer.color = Color.blue;
                    break;
                default:
                    break;
            }
        }

        public void Initialize(FoodSpawner parentSpawner)
        {
            _parentSpawner = parentSpawner;
        }

        public void OnEaten()
        {
            if (_parentSpawner) _parentSpawner.OnFoodEaten();

            Destroy(gameObject);
        }
    }
}