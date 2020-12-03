using System;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.MonoBehaviours
{
    [RequireComponent(typeof(IDamageable))]
    public class Damageable : MonoBehaviour
    {
        [Serializable]
        public class HealthEvent : UnityEvent<Damageable>
        {
        }

        [Serializable]
        public class DamageEvent : UnityEvent<Damager, Damageable>
        {
        }

        [Serializable]
        public class HealEvent : UnityEvent<int, Damageable>
        {
        }
        
        public bool invulnerableAfterDamage = true;
        public float invulnerabilityDuration = 3f;
        public bool disableOnDeath = false;
        public HealthEvent OnHealthChanged;
        public DamageEvent OnTakeDamage;
        public DamageEvent OnDie;
        public HealEvent OnGainHeal;

        [SerializeField] protected IDamageable _damageableModel;
        protected bool _invulnerable;
        protected float _inulnerabilityTimer;

        public int CurrentHealth => _damageableModel.CurrentHealth;

        private void Awake()
        {
            _damageableModel = GetComponent<IDamageable>();
        }

        private void Start()
        {
            _damageableModel.CurrentHealth = _damageableModel.StartingHealth;
            OnHealthChanged?.Invoke(this);
        }
        private void Update()
        {
            if (_invulnerable)
            {
                _inulnerabilityTimer -= Time.deltaTime;

                if (_inulnerabilityTimer <= 0f)
                {
                    _invulnerable = false;
                }
            }
        }

        public void EnableInvulnerability(bool ignoreTimer = false)
        {
            _invulnerable = true;
            _inulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
        }

        public void DisableInvulnerability()
        {
            _invulnerable = false;
        }
        
        public void TakeDamage(Damager damager, bool ignoreInvincible = false)
        {
            if ((_invulnerable && !ignoreInvincible) || _damageableModel.CurrentHealth <= 0)
                return;

            if (!_invulnerable)
            {
                _damageableModel.CurrentHealth -= damager.CountDamage;
                if (_damageableModel.CurrentHealth < 0)
                {
                    _damageableModel.CurrentHealth = 0;
                }
                OnHealthChanged?.Invoke(this);
            }

            OnTakeDamage?.Invoke(damager, this);

            if (invulnerableAfterDamage)
            {
                _invulnerable = true;
                _inulnerabilityTimer = invulnerabilityDuration;
            }

            if (_damageableModel.CurrentHealth <= 0)
            {
                OnDie?.Invoke(damager, this);
                EnableInvulnerability();
                if (disableOnDeath) gameObject.SetActive(false);
            }
        }

        public void GainHealth(int amount)
        {
            _damageableModel.CurrentHealth += amount;

            if (_damageableModel.CurrentHealth > _damageableModel.StartingHealth)
                _damageableModel.CurrentHealth = _damageableModel.StartingHealth;
            
            OnHealthChanged?.Invoke(this);
            OnGainHeal?.Invoke(amount, this);
        }

        public void SetHealth(int amount)
        {
            _damageableModel.CurrentHealth = amount;

            if (_damageableModel.CurrentHealth <= 0)
            {
                OnDie?.Invoke(null, this);
                EnableInvulnerability();
                if (disableOnDeath) gameObject.SetActive(false);
            }
            
            OnHealthChanged?.Invoke(this);
        }
    }
}
