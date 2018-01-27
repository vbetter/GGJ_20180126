using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : EnemyBehaviour
{
    bool m_isChoose = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_isChoose = false;

        base.OnStateEnter(animator, stateInfo, layerIndex);

        m_self.CurrentState = eActorState.Idle;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        //如果该单位有攻击技能，并且满足攻击条件
        if(m_self.HasAttack && m_self.EnableAttack())
        {
            m_self.SetState(eActorState.Attack);
            return;
        }
        else
        {
            if (m_stayTime > 0.5f && !m_isChoose)
            {
                m_isChoose = true;


                m_self.SetState(eActorState.Patrol);
            }
        }

    }

    bool isJump()
    {

        if(m_self.LastState == eActorState.Run )
        {
            m_self.OnJump();
            /*
            if (m_self.m_headHasPlatform)
            {
                m_self.OnJump();
            }
            */
        }
        return false;
    }
}
