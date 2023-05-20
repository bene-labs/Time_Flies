using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public enum Type
        {
            Red,
            Blue,
            Green
        }
        
        public Type type;
        public float size;
        public float eatValue;
        
        private SpriteRenderer _spriteRenderer;
        
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
        
    }
}