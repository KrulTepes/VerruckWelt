using System;
using Entities.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Environments
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactor : MonoBehaviour
    {
        public InputPlayer inputPlayer;
        public UnityEvent OnInteraction;
        public UnityEvent OnPlayerTriggerEnter;
        public UnityEvent OnPlayerTriggerExit;
        
        protected Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerTriggerEnter?.Invoke();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (inputPlayer.Attack)
            {
                _collider.enabled = false;
                OnInteraction?.Invoke();
                OnPlayerTriggerExit?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerTriggerExit?.Invoke();
            }
        }
    }
}