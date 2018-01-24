using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGame
{
    /// <summary>
    /// 飞行单位追击
    /// </summary>
    public class FlyChaseBehaviour : EnemyBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_target = GetNearTarget();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if(m_target == null)
            {
                m_self.SetState(eActorState.Idle);
            }
            else
            {
                Vector3 myPos = m_self.transform.localPosition;
                Vector3 pos = (m_target.transform.localPosition - myPos ) * Time.deltaTime*0.3f;
                m_self.transform.Translate(pos, Space.Self);
            }
        }

    }
}
