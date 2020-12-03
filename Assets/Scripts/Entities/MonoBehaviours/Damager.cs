using System;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.MonoBehaviours
{
    [RequireComponent(typeof(IDamager))]
    public class Damager : MonoBehaviour
    {
        [Serializable]
        public class DamageableEvent : UnityEvent<Damager, Damageable>
        { }
        
        [Serializable]
        public class NonDamageableEvent : UnityEvent<Damager>
        { }
        public float startDelayBetweenDamage = 0f;
        [Header("Area damage")]
        public Vector2 offset = new Vector2(1.5f, 1f);
        public Vector2 size = new Vector2(2.5f, 1f);
        [Header("Other collider (instead Size|Offset)")]
        public bool useColliderComponent = false;
        public bool cyclicalDamage = false;
        [Header("Other settings")]
        public bool offsetBasedOnSpriteFacing;
        public CharacterController2D controller;
        public bool canHitTriggers;
        public LayerMask hittableLayers;
        public DamageableEvent OnDamageableHit;
        public NonDamageableEvent OnNonDamageableHit;

        protected bool _spriteOriginallyFlipped = true;
        protected bool m_CanDamage = true;
        protected ContactFilter2D _attackContactFilter;
        protected Collider2D[] _attackOverlapResults = new Collider2D[10];
        protected Transform _damagerTransform;
        protected Collider2D _lastHit;
        [SerializeField] protected IDamager _damagerModel;
        [SerializeField] protected float _delayBetweenDamage;

        public int CountDamage => _damagerModel.Damage;

        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2 scale = transform.lossyScale;
            
            Vector2 facingOffset = Vector2.Scale(offset, scale);
            if (offsetBasedOnSpriteFacing && controller != null && controller.FacingRight != _spriteOriginallyFlipped)
                facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

            Vector2 scaledSize = Vector2.Scale(size, scale);
            Gizmos.DrawWireCube(transform.position + new Vector3(facingOffset.x, facingOffset.y, transform.position.z), 
                new Vector3(scaledSize.x, scaledSize.y, 0f));
        }

        #endregion
        private void Awake()
        {
            _damagerModel = GetComponent<IDamager>();
            
            _attackContactFilter.layerMask = hittableLayers;
            _attackContactFilter.useLayerMask = true;
            _attackContactFilter.useTriggers = canHitTriggers;

            if (offsetBasedOnSpriteFacing && controller != null)
                _spriteOriginallyFlipped = controller.FacingRight;
            
            _damagerTransform = transform;
        }

        private void Update()
        {
            _delayBetweenDamage -= Time.deltaTime;
            if (_delayBetweenDamage <= 0f)
            {
                EnableDamage();
            }
            else
            {
                DisableDamage();
            }
        }

        public void EnableDamage()
        {
            m_CanDamage = true;
        }

        public void DisableDamage()
        {
            m_CanDamage = false;
        }

        public void Damage()
        {
            if (!m_CanDamage)
                return;
            if (useColliderComponent)
                return;
            
            Vector2 scale = _damagerTransform.lossyScale;
            
            Vector2 facingOffset = Vector2.Scale(offset, scale);
            if (offsetBasedOnSpriteFacing && controller != null && controller.FacingRight != _spriteOriginallyFlipped)
                facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

            Vector2 scaledSize = Vector2.Scale(size, scale);

            Vector2 pointA = (Vector2) _damagerTransform.position + facingOffset - scaledSize * 0.5f;
            Vector2 pointB = pointA + scaledSize;

            int hitCount = Physics2D.OverlapArea(pointA, pointB, _attackContactFilter, _attackOverlapResults);

            for (int i = 0; i < hitCount; i++)
            {
                _lastHit = _attackOverlapResults[i];
                Damageable damageable = _lastHit.GetComponent<Damageable>();

                ToDamage(damageable);
            }
        }

        private void ToDamage(Damageable damageable)
        {
            if (damageable)
            {
                _delayBetweenDamage = startDelayBetweenDamage;
                OnDamageableHit?.Invoke(this, damageable);
                damageable.TakeDamage(this);
            }
            else
            {
                OnNonDamageableHit?.Invoke(this);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_CanDamage)
                return;
            
            if (hittableLayers == (hittableLayers | (1 << other.gameObject.layer)))
            {
                Damageable damageable = other.GetComponent<Damageable>();
                ToDamage(damageable);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!m_CanDamage)
                return;
            if (!cyclicalDamage)
                return;
            
            Damageable damageable = other.GetComponent<Damageable>();
            ToDamage(damageable);
        }

        // private void FixedUpdate()
        // {
        //     if (!m_CanDamage)
        //         return;
        //     
        //     Vector2 scale = _damagerTransform.lossyScale;
        //     
        //     Vector2 facingOffset = Vector2.Scale(offset, scale);
        //     if (offsetBasedOnSpriteFacing && controller != null && controller.FacingRight != _spriteOriginallyFlipped)
        //         facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);
        //
        //     Vector2 scaledSize = Vector2.Scale(size, scale);
        //
        //     Vector2 pointA = (Vector2) _damagerTransform.position + facingOffset - scaledSize * 0.5f;
        //     Vector2 pointB = pointA + scaledSize;
        //
        //     int hitCount = Physics2D.OverlapArea(pointA, pointB, _attackContactFilter, _attackOverlapResults);
        //
        //     for (int i = 0; i < hitCount; i++)
        //     {
        //         _lastHit = _attackOverlapResults[i];
        //         Damageable damageable = _lastHit.GetComponent<Damageable>();
        //
        //         if (damageable)
        //         {
        //             OnDamageableHit?.Invoke(this, damageable);
        //             damageable.TakeDamage(this);
        //         }
        //         else
        //         {
        //             OnNonDamageableHit?.Invoke(this);
        //         }
        //     }
        // }
    }
}
