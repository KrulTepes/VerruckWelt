using Entities.MonoBehaviours;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Player
{
    public class MeleeAttackSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.UseMeleeAttack();
        }
    }
}
