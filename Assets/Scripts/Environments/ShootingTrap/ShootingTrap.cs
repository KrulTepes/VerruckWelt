using UnityEngine;

namespace Environments.ShootingTrap
{
    public class ShootingTrap : MonoBehaviour
    {
        public float cooldown;
        public GameObject arrowObject;

        protected float _localCooldown;
        
        void FixedUpdate()
        {
            _localCooldown -= Time.fixedDeltaTime;

            if (_localCooldown <= 0f)
            {
                Instantiate(arrowObject, transform.position, Quaternion.identity, transform.parent);
                _localCooldown = cooldown;
            }
        }
    }
}
