using UnityEngine;
using UnityEngine.Events;

namespace Environments.ShootingTrap
{
    public class Arrow : MonoBehaviour
    {
        public float lifeTime;
        public float timeSurprise;
        public float speed;
        public bool isLeft;

        public UnityEvent OnSurprise;
        
        protected float _localLifeTime;
        protected float _localTimeSurprise;

        protected bool _surpriseHappened = false;
        
        void Start()
        {
            _localLifeTime = lifeTime;
            _localTimeSurprise = timeSurprise;
        }
        
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + (isLeft ? -1 : 1) * speed, 
                transform.position.y, transform.position.z), Time.fixedDeltaTime);
            
            _localLifeTime -= Time.fixedDeltaTime;
            _localTimeSurprise -= Time.fixedDeltaTime;

            if (_localTimeSurprise <= 0f && _surpriseHappened == false)
            {
                OnSurprise?.Invoke();
                _surpriseHappened = true;
            }

            if (_localLifeTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
