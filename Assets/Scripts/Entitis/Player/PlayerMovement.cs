using UnityEngine;

namespace Entitis.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController2D controller;
        public Animator animator;
        public InputPlayer inputPlayer;

        public float runSpeed = 40f;
    
        private float _horizontalMove = 0f;
        private bool _jump = false;
    
        private void Update()
        {
            _horizontalMove = inputPlayer.Horizontal * runSpeed;
            animator.SetFloat("HorizontalMove", Mathf.Abs(_horizontalMove));
        
            if (inputPlayer.Vertical == Vector2.up.y) {
                _jump = true;
                animator.SetBool("Jump", true);
            }
        }

        public void OnLanding()
        {
            animator.SetBool("Jump", false);
        }

        private void FixedUpdate()
        {
            controller.Move(_horizontalMove * Time.deltaTime, false, _jump);
            _jump = false;
        }
    }
}
