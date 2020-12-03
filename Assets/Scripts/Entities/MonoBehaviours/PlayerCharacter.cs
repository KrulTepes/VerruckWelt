using Entities.MonoBehaviours.Player;
using Entities.StateMachineBehaviours;
using UI;
using UnityEngine;

namespace Entities.MonoBehaviours
{
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InputPlayer))]
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerCharacter : MonoBehaviour
    {
        static protected PlayerCharacter _PlayerInstance;

        static public PlayerCharacter PlayerInstance
        {
            get { return _PlayerInstance; }
        }

        public Damager meleeAttack;
        public Damageable damageable;

        public Transform RespawnPoint;

        protected PlayerModel _playerModel;
        protected Animator _animator;
        protected InputPlayer _inputPlayer;
        protected CharacterController2D _controller;

        protected readonly int _HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
        protected readonly int _HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
        protected readonly int _HashGroundedPara = Animator.StringToHash("Grounded");
        protected readonly int _HashHurtPara = Animator.StringToHash("Hurt");
        protected readonly int _HashDeadPara = Animator.StringToHash("Dead");
        protected readonly int _HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");

        private void Awake()
        {
            _PlayerInstance = this;

            _playerModel = GetComponent<PlayerModel>();
            _controller = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
            _inputPlayer = GetComponent<InputPlayer>();
        }

        private void Start()
        {
            meleeAttack.EnableDamage();
            SceneLinkedSMB<PlayerCharacter>.Initialise(_animator, this);
        }

        private void FixedUpdate()
        {
            _controller.Move(_inputPlayer.Horizontal * _playerModel.RunSpeed * Time.fixedDeltaTime, false,
                _inputPlayer.Vertical > 0);
            _animator.SetFloat(_HashHorizontalSpeedPara, _controller.MoveVector.x);
            _animator.SetFloat(_HashVerticalSpeedPara, _controller.MoveVector.y);
        }

        #region Checking Behaviours

        public bool CheckForGrounded()
        {
            bool grounded = _controller.IsGrounded;
            _animator.SetBool(_HashGroundedPara, grounded);
            return grounded;
        }

        public bool CheckForMeleeAttackInput()
        {
            return _inputPlayer.Attack;
        }

        #endregion

        public void OnHurt(Damager damager, Damageable damageable)
        {
            damageable.EnableInvulnerability();

            _animator.SetTrigger(_HashHurtPara);
        }

        public void OnDie()
        {
            _animator.SetTrigger(_HashDeadPara);
            StartCoroutine(UIController.FadeSceneOut(UIController.FadeType.Death));
        }

        public void SetMoveVector(Vector2 newMoveVector)
        {
            _controller.MoveVector = newMoveVector;
        }

        public bool IsFalling()
        {
            return _controller.MoveVector.y < 0f && !_animator.GetBool(_HashGroundedPara);
        }

        public void UseMeleeAttack()
        {
            meleeAttack.Damage();
        }

        public void MeleeAttack()
        {
            _animator.SetTrigger(_HashMeleeAttackPara);
        }
    }
}
