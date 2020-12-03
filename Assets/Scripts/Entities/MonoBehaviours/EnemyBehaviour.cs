using System;
using System.Net;
using Entities.MonoBehaviours.SkeletonHI;
using Entities.StateMachineBehaviours;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Entities.MonoBehaviours
{
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SkeletonHeavyInfantry))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Scanning settings")]
        [Tooltip("The angle of the forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.")]
        [Range(0.0f,360.0f)]
        public float viewDirection = 0.0f;
        [Range(0.0f, 360.0f)]
        public float viewFov;
        public float viewDistance;
        [Tooltip("Time in seconds without the target in the view cone before the target is considered lost from sight")]
        public float timeBeforeTargetLost = 3.0f;
        
        [Header("Melee Attack Data")]
        [EnemyMeleeRangeCheck]
        public float meleeRange = 3.0f;
        public Damager meleeDamager;

        public float EstimatedSpeed => _moveVector.x * _skeletonHIModel.RunSpeed * Time.deltaTime;
        public float LastSpeed;
        protected Animator _animator;
        protected Collider2D _collider;
        protected CharacterController2D _controller;
        protected SkeletonHeavyInfantry _skeletonHIModel;
        
        protected Vector3 _moveVector;
        protected Transform _target;
        [SerializeField] protected float _timeSinceLastTargetView;
        protected float _fireTimer = 0.0f;
        
        protected Bounds _localBounds;
        
        protected bool _dead = false;
        
        protected readonly int _HashSpottedPara = Animator.StringToHash("Spotted");
        protected readonly int _HashTargetLostPara = Animator.StringToHash("TargetLost");
        protected readonly int _HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");
        protected readonly int _HashHitPara = Animator.StringToHash("Hit");
        protected readonly int _HashDeathPara = Animator.StringToHash("Death");
        protected readonly int _HashGroundedPara = Animator.StringToHash("Grounded");

        private void Awake()
        {
            _skeletonHIModel = GetComponent<SkeletonHeavyInfantry>(); // нужно сделать общай класс врагов
            _controller = GetComponent<CharacterController2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _dead = false;
            _collider.enabled = true;
            
            _moveVector = Vector3.right;
            meleeDamager.EnableDamage();
            
            Collider2D[] _colliderCache = new Collider2D[16];
            _localBounds = new Bounds();
            int count = _controller.Rigidbody2D.GetAttachedColliders(_colliderCache);
            for (int i = 0; i < count; ++i)
            {
                _localBounds.Encapsulate(transform.InverseTransformBounds(_colliderCache[i].bounds));
            }
            SceneLinkedSMB<EnemyBehaviour>.Initialise(_animator, this);
        }

        private void FixedUpdate()
        {
            if (_dead)
                return;

            _controller.Move(_moveVector.x * _skeletonHIModel.RunSpeed * Time.fixedDeltaTime, false, _moveVector.y > 0);

            UpdateTimers();
            
            _animator.SetBool(_HashGroundedPara, _controller.IsGrounded);
        }

        private void UpdateTimers()
        {
            if (_timeSinceLastTargetView > 0f)
            {
                _timeSinceLastTargetView -= Time.fixedDeltaTime;
            }

            if (_fireTimer > 0f)
            {
                _fireTimer -= Time.fixedDeltaTime;
            }
        }

        #region Checking Behaviours
        
        public bool CheckForTargetIsRight()
        {
            return _target.position.x >= transform.position.x;
        }

        public bool CheckTargetMeleeAttackRange()
        {
            return (_target.transform.position - transform.position).sqrMagnitude < Mathf.Pow(meleeRange, 2);
        }

        public bool CheckForObstacle(float forwardDistance)
        {
            if (Physics2D.CircleCast(_collider.bounds.center, _collider.bounds.extents.y - 0.2f, 
                _controller.FacingDirection, forwardDistance, _controller.WhatIsGround.value))
            {
                return true;
            }

            Vector3 castingPosition = (Vector2) (transform.position + _localBounds.center) +
                                      _controller.FacingDirection * _localBounds.extents.x;
            Debug.DrawLine(castingPosition, castingPosition + Vector3.down * (_localBounds.extents.y + 0.2f));

            if (!Physics2D.CircleCast(castingPosition, 0.1f, Vector2.down, 
                _localBounds.extents.y + 0.2f, _controller.WhatIsGround.value))
            {
                return true;
            }

            return false;
        }

        public bool CheckTargetStillVisible()
        {
            if (_target == null)
                return false;

            Vector3 toTarget = _target.position - transform.position;

            if (toTarget.sqrMagnitude < Mathf.Pow(viewDistance, 2))
            {
                Vector3 testForward = Quaternion.Euler(0, 0, _controller.FacingRight ? viewDirection : -viewDirection) * _controller.FacingDirection;

                float angle = Vector3.Angle(testForward, toTarget);

                if (angle <= viewFov * 0.5f)
                {
                    //we reset the timer if the target is at viewing distance.
                    _timeSinceLastTargetView = timeBeforeTargetLost;
                }
            }
            
            if (_timeSinceLastTargetView <= 0.0f)
            {
                ForgetTarget();
                return false;
            }
            return true;
        }
        #endregion

        public void MeleeAttack()
        {
            _animator.SetTrigger(_HashMeleeAttackPara);
        }
        public void ForgetTarget()
        {
            _animator.SetTrigger(_HashTargetLostPara);
            _target = null;
        }
        
        public void ScanForPlayer()
        {
            Vector3 direction = PlayerCharacter.PlayerInstance.transform.position - transform.position;

            if (direction.sqrMagnitude > Mathf.Pow(viewDistance, 2))
            {
                return;
            }

            Vector3 testForward =
                Quaternion.Euler(0, 0,
                    Mathf.Sign(_controller.FacingDirection.x) *
                    (_controller.FacingRight ? viewDirection : -viewDirection)) * _controller.FacingDirection;

            float angle = Vector3.Angle(testForward, direction);

            if (angle > viewFov * 0.5f)
            {
                return;
            }

            _target = PlayerCharacter.PlayerInstance.transform;
            _timeSinceLastTargetView = timeBeforeTargetLost;
            
            _animator.SetTrigger(_HashSpottedPara);
        }
            
        public void SetHorizontalSpeed(float direction)
        {
            if (direction == 0f)
            {
                _moveVector.x = 0;
                return;
            }

            _moveVector.x = direction > 0 ? Vector2.right.x : Vector2.left.x;
            
            LastSpeed = direction;
        }

        public void UseMeleeAttack()
        {
            meleeDamager.Damage();
        }

        public void Die(Damager damager, Damageable damageable)
        {
            _animator.SetTrigger(_HashDeathPara);
            _dead = true;
        }

        public void Hit(Damager damager, Damageable damageable)
        {
            if (damageable.CurrentHealth <= 0)
                return;

            _animator.SetTrigger(_HashHitPara);
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            CharacterController2D localController = GetComponent<CharacterController2D>();
            //draw the cone of view
            Vector3 forward = localController.FacingRight ? Vector2.right : Vector2.left;
            forward = Quaternion.Euler(0, 0, localController.FacingRight ? viewDirection : -viewDirection) * forward;

            if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

            Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = new Color(0, 1.0f, 0, 0.2f);
            Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

            //Draw attack range
            Handles.color = new Color(1.0f, 0,0, 0.1f);
            Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
        }
        #endif
    }
    
    //bit hackish, to avoid to have to redefine the whole inspector, we use an attirbute and associated property drawer to 
    //display a warning above the melee range when it get over the view distance.
    public class EnemyMeleeRangeCheckAttribute : PropertyAttribute
    {

    }

    #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(EnemyMeleeRangeCheckAttribute))]
        public class EnemyMeleeRangePropertyDrawer : PropertyDrawer
        {

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                SerializedProperty viewRangeProp = property.serializedObject.FindProperty("viewDistance");
                if (viewRangeProp.floatValue < property.floatValue)
                {
                    Rect pos = position;
                    pos.height = 30;
                    EditorGUI.HelpBox(pos, "Melee range is bigger than View distance. Note enemies only attack if target is in their view range first", MessageType.Warning);
                    position.y += 30;
                }

                EditorGUI.PropertyField(position, property, label);
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                SerializedProperty viewRangeProp = property.serializedObject.FindProperty("viewDistance");
                if (viewRangeProp.floatValue < property.floatValue)
                {
                    return base.GetPropertyHeight(property, label) + 30;
                }
                else
                    return base.GetPropertyHeight(property, label);
            }
        }
    #endif
}
