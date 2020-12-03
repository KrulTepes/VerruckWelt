using Entities.MonoBehaviours;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Player
{
    public class AirborneSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CheckForGrounded();
            if (m_MonoBehaviour.CheckForMeleeAttackInput())
                m_MonoBehaviour.MeleeAttack();
        }
    }
}
