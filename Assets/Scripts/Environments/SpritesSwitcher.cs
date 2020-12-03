using UnityEngine;

namespace Environments
{
    public class SpritesSwitcher : MonoBehaviour
    {
        public Sprite switchedSprite;

        protected SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Switch()
        {
            _spriteRenderer.sprite = switchedSprite;
        }
    }
}