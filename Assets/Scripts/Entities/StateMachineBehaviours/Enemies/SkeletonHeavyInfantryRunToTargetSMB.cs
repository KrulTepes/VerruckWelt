using Entities.MonoBehaviours;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Enemies
{
    public class SkeletonHeavyInfantryRunToTargetSMB : SceneLinkedSMB<EnemyBehaviour>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

            if (!m_MonoBehaviour.CheckTargetStillVisible())
            {
                m_MonoBehaviour.ForgetTarget();
                return;
            }

            if (m_MonoBehaviour.CheckTargetMeleeAttackRange())
            {
                m_MonoBehaviour.MeleeAttack();
            }
            else
            {
                float speed = m_MonoBehaviour.EstimatedSpeed;
                if (speed == 0f)
                    speed = m_MonoBehaviour.LastSpeed;
                
                if (m_MonoBehaviour.CheckForTargetIsRight())
                {
                    speed = Mathf.Abs(speed);
                }
                else
                {
                    speed = Mathf.Abs(speed) * -1;
                }

                if (m_MonoBehaviour.CheckForObstacle(speed))
                {
                    m_MonoBehaviour.ForgetTarget();
                    return;
                }
                
                m_MonoBehaviour.SetHorizontalSpeed(speed);
            }
        }
    }
}