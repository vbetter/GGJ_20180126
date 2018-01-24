using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : BaseBehaviour
{
    protected Actor m_target;//敌人

    protected Enemy m_self;//我自己

    protected Rigidbody2D m_Rigidbody2D;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_self = animator.GetComponent<Enemy>();
        m_Rigidbody2D = animator.GetComponent<Rigidbody2D>();

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        m_self.DisableCurrentState();
    }

    /// <summary>
    /// 获取最近的敌人
    /// </summary>
    /// <returns></returns>
    protected Actor GetNearTarget()
    {
        float distance = 999;
        List<Player>  players= LevelManager.Instance.AllPlayerList;
        Vector2 myPos = new Vector2(m_self.transform.localPosition.x, m_self.transform.localPosition.y);
        Actor target = null;
        foreach (var item in players)
        {
            float dis = Vector2.Distance(new Vector2(item.transform.localPosition.x, item.transform.localPosition.y), myPos);
            if(dis<=distance && item.CurrentState!= eActorState.Die && item.CurrentState!= eActorState.Coma)
            {
                distance = dis;
                target = item;
            }
        }

        return target;
    }

    public Vector2 MyPosition
    {
        get
        {
            return new Vector2(m_self.transform.localPosition.x, m_self.transform.localPosition.y);
        }
    }
}
