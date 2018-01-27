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
        //攻击间隔时间
        float m_checkTargeTimer = 0;
        float m_checkTargeMax = 1;

        Vector3 m_randomPos = Vector3.zero;//随机移动的坐标点



        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_target = GetNearTarget();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

			if(m_self.IsEnableMove())
			{
				Vector3 start = m_self.m_startPos;
				Vector3 end = m_self.m_endPos;

				if(m_self.transform.localPosition.x == m_self.m_endPos.x && m_self.transform.localScale.x>0)
				{
					//转身
					m_self.Flip();

					end = m_self.m_startPos;

				}else if(m_self.transform.localPosition.x == m_self.m_startPos.x && m_self.transform.localScale.x<0)
				{
					//转身
					m_self.Flip();

					end = m_self.m_endPos;

				}
					
				//向目标移动
	

				m_self.transform.localPosition=new Vector3(Mathf.Lerp(start.x, end.x, Time.time*0.1f), 
					m_self.transform.localPosition.y, m_self.transform.localPosition.z);

			}

			/*
			 * 
            if(m_checkTargeTimer<m_checkTargeMax)
            m_checkTargeTimer += Time.deltaTime;


            if (m_target == null)
            {
                //如果目标死亡，换一个
                if (m_checkTargeTimer >= m_checkTargeMax)
                {
                    m_checkTargeTimer = 0;
                    //如果目标死亡，换一个
                    m_target = GetNearTarget();
                }

                //如果还没找到目标待机
                if(m_target==null)
                {
                    if(m_randomPos == Vector3.zero || m_randomPos== m_self.transform.position)
                    {
                        m_randomPos = GetRandomPosition();
                    }

                    //向目标移动
                    Vector3 myPos = m_self.transform.localPosition;
                    Vector3 pos = (m_randomPos - myPos) * Time.deltaTime * 0.3f;
                    m_self.transform.Translate(pos, Space.Self);

                }
            }
            else
            {
                if (m_target.CurrentState != eActorState.Die && m_target.CurrentState != eActorState.Coma)
                {
                    //向目标移动
                    Vector3 myPos = m_self.transform.localPosition;
                    Vector3 pos = (m_target.transform.localPosition - myPos) * Time.deltaTime * 0.3f;
                    m_self.transform.Translate(pos, Space.Self);
                }
                else
                {
                    if (m_checkTargeTimer >= m_checkTargeMax)
                    {
                        m_checkTargeTimer = 0;
                        //如果目标死亡，换一个
                        m_target = GetNearTarget();
                    }
                }           
            }

			*/

        }

        /// <summary>
        /// 获取一个随机目标点
        /// </summary>
        /// <returns></returns>
        Vector3 GetRandomPosition()
        {
            float x = Random.Range(-12, 12);
            float y = Random.Range(-5f, 5f);
            Vector3 pos = new Vector3(x, y, 0);

            return pos;
        }
    }
}
