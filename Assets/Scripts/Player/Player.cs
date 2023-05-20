using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public float size;
        private SpriteRenderer _spriteRenderer;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var sprite = _spriteRenderer.sprite;
            var localScale = transform.localScale;
            size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
