using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : EnemyBehaviour
{
    //攻击间隔时间
    float m_attackTimer = 0;
    float m_attacktMax = 3;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        m_target = GetNearTarget();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (m_attackTimer <= m_attacktMax)
            m_attackTimer += Time.deltaTime;

        if (m_stayTime>3f)
        {
            if(m_attackTimer>= m_attacktMax)
            {
                m_attackTimer = 0;
                m_self.Attack();
            }

            //如果不满足攻击条件
            if(!m_self.EnableAttack())
            {
                m_self.SetState(eActorState.Idle);
            }
        }

    }
}
