using UI;
using UnityEngine;

namespace Entitis.Player
{
    public class InputPlayer : MonoBehaviour
    {
        [SerializeField] public ButtonUtility LeftMove;
        [SerializeField] public ButtonUtility RightMove;
        [SerializeField] public ButtonUtility Jump;
        [SerializeField] public ButtonUtility Hit;
        [SerializeField] public ButtonUtility Shot;
        [SerializeField] public ButtonUtility Shield;

        private float _horizontal = 0f;
        private float _vertical = 0f;
        private bool _isLeftMove = false;
        private bool _isRightMove = false;
    
        public float Horizontal { get => _horizontal; }
        public float Vertical { get => _vertical; }
    
        private void Start()
        {
            LeftMove.OnDown += LeftMoveOnDown;
            LeftMove.OnUp += LeftMoveOnUp;
            RightMove.OnDown += RightMoveOnDown;
            RightMove.OnUp += RightMoveOnUp;
            Jump.OnDown += JumpOnDown;
            Jump.OnUp += JumpOnUp;
        }

        private void JumpOnUp()
        {
            _vertical -= 1f;
        }
        private void JumpOnDown()
        {
            _vertical += 1f;
        }
        private void RightMoveOnUp()
        {
            _horizontal -= 1f;
        }
        private void RightMoveOnDown()
        {
            _horizontal += 1f;
        }
        private void LeftMoveOnUp()
        {
            _horizontal += 1f;
        }
        private void LeftMoveOnDown()
        {
            _horizontal -= 1f;
        }
    }
}
