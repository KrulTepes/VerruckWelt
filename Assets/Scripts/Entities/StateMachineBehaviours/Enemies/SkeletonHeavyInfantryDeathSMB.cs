using Entities.MonoBehaviours;
using UnityEngine;
using UnityEngine.Animations;

namespace Entities.StateMachineBehaviours.Enemies
{
    public class SkeletonHeavyInfantryDeathSMB : SceneLinkedSMB<EnemyBehaviour>
    {
        public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.gameObject.SetActive(false);
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(m_MonoBehaviour.gameObject);
        }
    }
}