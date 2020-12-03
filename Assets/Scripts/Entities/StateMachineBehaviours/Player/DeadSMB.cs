using Entities.MonoBehaviours;
using TMPro;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Player
{
    public class DeadSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateNoTransitionUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CheckForGrounded();
        }
        
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.SetMoveVector(Vector2.zero);
        }
    }
}
