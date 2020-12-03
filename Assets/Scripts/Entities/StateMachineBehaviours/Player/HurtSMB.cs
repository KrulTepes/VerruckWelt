using Entities.MonoBehaviours;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Player
{
    public class HurtSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateNoTransitionUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(m_MonoBehaviour.IsFalling ())
                m_MonoBehaviour.CheckForGrounded();
        }
    }
}
