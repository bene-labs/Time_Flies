using UnityEngine;

namespace Food
{
    public class Food : MonoBehaviour
    {
        public enum Type
        {
            Mixed,
            Greasy,
            Sweet,
            Spoiled
        }

        public Type type;
       // public float size;

        public float energyValue;
        //public float eatSizeValue;

        private SpriteRenderer _spriteRenderer;
        private FoodSpawner _parentSpawner;

        private void Start()
        {
            /*
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var sprite = _spriteRenderer.sprite;
            var localScale = transform.localScale;
            size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;*/
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