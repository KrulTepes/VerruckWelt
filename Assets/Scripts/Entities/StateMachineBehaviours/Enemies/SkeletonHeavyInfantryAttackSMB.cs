using Entities.MonoBehaviours;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Enemies
{
    public class SkeletonHeavyInfantryAttackSMB : SceneLinkedSMB<EnemyBehaviour>
    {
        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStatePostEnter(animator, stateInfo, layerIndex);
        
            m_MonoBehaviour.UseMeleeAttack();
        }
        
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);
            
            m_MonoBehaviour.SetHorizontalSpeed(0);
            m_MonoBehaviour.ScanForPlayer();
        }
    }
}