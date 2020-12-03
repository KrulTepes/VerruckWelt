using System;
using UnityEngine;

namespace Environments
{
    public class DisappearingPlatform : MonoBehaviour
    {
        public float timeBeforeDisappearing;
        public float timeDisappearing;
        public float forceShaking;

        public GameObject Platform;

        protected Collider2D _collider;
        protected float _localTimeBeforeDisappearing = -1f;
        protected float _localTimeDisappearing = -1f;
        protected bool _shakeLeft = true;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            if (_localTimeBeforeDisappearing >= 0f)
            {
                transform.position = new Vector3(transform.position.x + forceShaking * (_shakeLeft ? 1 : -1) * Time.fixedDeltaTime, transform.position.y, transform.position.z);
                _shakeLeft = !_shakeLeft;
                _localTimeBeforeDisappearing -= Time.fixedDeltaTime;
                if (_localTimeBeforeDisappearing <= 0f)
                {
                    _localTimeDisappearing = timeDisappearing;
                    _collider.enabled = false;
                    Platform.SetActive(false);
                }
            }
            
            if (_localTimeDisappearing >= 0f)
            {
                _localTimeDisappearing -= Time.fixedDeltaTime;
                if (_localTimeDisappearing <= 0f)
                {
                    _collider.enabled = true;
                    Platform.SetActive(true);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_localTimeBeforeDisappearing <= 0f && _localTimeDisappearing <= 0f) {
                    _localTimeBeforeDisappearing = timeBeforeDisappearing;
                }
            }
        }
    }
}