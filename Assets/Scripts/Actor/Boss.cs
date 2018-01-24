using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {

    public int AttackRange = 500;//攻击范围

    [SerializeField]
    GameObject m_cloneBullet;

    List<Vector3> m_dirsList = new List<Vector3>();

	// Use this for initialization
	void Start () {
        HasAttack = true;

        m_dirsList.Add(new Vector3(-1f, 1, 0));
        m_dirsList.Add(new Vector3(1f, -1, 0));
        m_dirsList.Add(new Vector3(-1f, -1, 0));
        m_dirsList.Add(new Vector3(1f,1,0));
        m_dirsList.Add(Vector3.up);
        m_dirsList.Add(Vector3.down);
        m_dirsList.Add(Vector3.left);
        m_dirsList.Add(Vector3.right);
    }

    public override bool EnableAttack()
    {
        Actor target = GetNearTarget();
        if(target!=null)
        {
            float distance = Vector2.Distance(transform.localPosition, target.transform.localPosition);
            if(distance<=AttackRange)
            {
                return true;
            }
        }
        return base.EnableAttack();
    }

    public override void Attack()
    {
        
        base.Attack();

        attack2();
    }

    /// <summary>
    /// 向角色发起定向攻击
    /// </summary>
    void attack1()
    {
        foreach (var item in LevelManager.Instance.AllPlayerList)
        {
            GameObject go = Instantiate(m_cloneBullet);
            if (go != null)
            {
                go.transform.localPosition = transform.localPosition;

                BossBullet bullect = go.GetComponent<BossBullet>();
                if (bullect != null)
                {
                    Actor target = item;
                    Vector3 dir = target.transform.localPosition - transform.localPosition;

                    bullect.Init(dir);
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    void attack2()
    {
        foreach (var item in m_dirsList)
        {
            Vector3 dir = item;

            GameObject go = Instantiate(m_cloneBullet);
            if (go != null)
            {
                go.transform.localPosition = transform.localPosition;

                BossBullet bullect = go.GetComponent<BossBullet>();
                if (bullect != null)
                {
                    bullect.Init(dir, 2f);
                }
            }
        }
    }
}
