using Entities.MonoBehaviours;
using UnityEngine;

namespace Entities.StateMachineBehaviours.Enemies
{
    public class SkeletonHeavyInfantryPatrolSMB : SceneLinkedSMB<EnemyBehaviour>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float dir = m_MonoBehaviour.EstimatedSpeed;
            if (dir == 0f) 
                dir = m_MonoBehaviour.LastSpeed;
            
            if (m_MonoBehaviour.CheckForObstacle(dir))
            {
                m_MonoBehaviour.SetHorizontalSpeed(-dir);
            }
            else
            {
                m_MonoBehaviour.SetHorizontalSpeed(dir);
            }
            
            m_MonoBehaviour.ScanForPlayer();
        }
    }
}