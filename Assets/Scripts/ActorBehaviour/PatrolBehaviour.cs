using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGame
{
    /// <summary>
    /// 巡逻
    /// </summary>
    public class PatrolBehaviour : EnemyBehaviour
    {
        //检测敌人间隔时间
        float m_checkTargetTimer = 0;
        float m_checkTargetMax = 3;

        //检测是否换操作间隔时间
        float m_checkControllerTimer = 0;
        float m_checkControllerMax = 1;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_checkControllerTimer = m_checkControllerMax;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if(m_checkTargetTimer<= m_checkTargetMax)
            m_checkTargetTimer += Time.deltaTime;

            if (m_checkControllerTimer <= m_checkControllerMax)
                m_checkControllerTimer += Time.deltaTime;

            if (m_target ==null || m_checkTargetTimer>= m_checkTargetMax)
            {
                m_target = GetNearTarget();
                m_checkTargetTimer = 0;
            }

            if(m_target==null)
            {
                MoveToFront();
            }
            else
            {
                if(m_checkControllerTimer >= m_checkControllerMax)
                {
                    m_checkControllerTimer = 0;

                    Vector2 targetPos = new Vector2(m_target.transform.localPosition.x, m_target.transform.localPosition.y);

                    if (targetPos.y > MyPosition.y && m_self.m_headHasPlatform)
                    {
                        m_self.SetState(eActorState.Jump);
                        return;
                    }
                    MoveToFront();
                }
                else
                {
                    MoveToFront();
                }

                
            }
        }


        /// <summary>
        /// 向前移动
        /// </summary>
        void MoveToFront()
        {

            float x = 0;
            if (m_Rigidbody2D.velocity.x > m_self.maxSpeed)
            {
                x = Mathf.Sign(m_Rigidbody2D.velocity.x) * m_self.maxSpeed;
            }
            else
            {
                x = m_self.transform.localScale.x * m_self.moveSpeed;
            }
            m_Rigidbody2D.velocity = new Vector2(x, m_Rigidbody2D.velocity.y);

            if (m_self.m_frontHasWall && m_self.IsGround)
            {
                m_self.Flip();
            }
        }
    }
}
