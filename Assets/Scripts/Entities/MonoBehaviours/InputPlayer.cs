using UI;
using UnityEngine;

namespace Entities.MonoBehaviours
{
    public class InputPlayer : MonoBehaviour
    {
        [SerializeField] public ButtonUtility LeftMoveButton;
        [SerializeField] public ButtonUtility RightMoveButton;
        [SerializeField] public ButtonUtility JumpButton;
        [SerializeField] public ButtonUtility AttackButton;
        [SerializeField] public ButtonUtility ShotButton;
        [SerializeField] public ButtonUtility ShieldButton;

        private float _horizontal = 0f;
        private float _vertical = 0f;
        private bool _attack = false;
        private bool _isLeftMove = false;
        private bool _isRightMove = false;
    
        public float Horizontal { get => _horizontal; }
        public float Vertical { get => _vertical; }
        public bool Attack { get => _attack; }
    
        private void Start()
        {
            LeftMoveButton.OnDown += LeftMoveButtonOnDown;
            LeftMoveButton.OnUp += LeftMoveButtonOnUp;
            RightMoveButton.OnDown += RightMoveButtonOnDown;
            RightMoveButton.OnUp += RightMoveButtonOnUp;
            JumpButton.OnDown += JumpButtonOnDown;
            JumpButton.OnUp += JumpButtonOnUp;
            AttackButton.OnDown += AttackOnDown;
            AttackButton.OnUp += AttackOnUp;
        }

        // Attack
        private void AttackOnUp()
        {
            _attack = false;
        }
        private void AttackOnDown()
        {
            _attack = true;
        }

        // Jump
        private void JumpButtonOnUp()
        {
            _vertical -= 1f;
        }
        private void JumpButtonOnDown()
        {
            _vertical += 1f;
        }
        
        // Right
        private void RightMoveButtonOnUp()
        {
            _horizontal -= 1f;
        }
        private void RightMoveButtonOnDown()
        {
            _horizontal += 1f;
        }
        
        // Left
        private void LeftMoveButtonOnUp()
        {
            _horizontal += 1f;
        }
        private void LeftMoveButtonOnDown()
        {
            _horizontal -= 1f;
        }
    }
}
