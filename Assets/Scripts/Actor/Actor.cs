using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eActorState
{
    None,
    Snow,
    Run,
    Die,
    Idle,
    Scroll,
    Jump,
    Patrol,//巡逻
    Coma,//昏迷
    Attack,//攻击
}

public enum eBuffState
{
    None,
    Invincible,//无敌
}

/// <summary>
/// 方位
/// </summary>
public enum ePosistion
{
    None,
    Up,
    Down,
    Left,
    Right,
    //LeftUp,
    //LeftDown,
    //RightUp,
    //RightDown,
}

public class Actor : MonoBehaviour
{

    protected Animator m_animator;

    public eActorState CurrentState = eActorState.None;

    public eActorState LastState = eActorState.None;

    public int JumpPower = 400;//跳跃高度

    bool m_isFaceToRight = true;//是否面向右边
    public bool IsFaceToRight
    {
        get
        {
            if(transform.localScale.x>0)
            {
                m_isFaceToRight = true;
            }
            else
            {
                m_isFaceToRight = false;
            }
            return m_isFaceToRight;
        }
    }

    protected int m_hp = 1;

    public int HP
    {
        get
        {
            return m_hp;
        }set
        {
            m_hp = value;

            m_hp = m_hp < 0 ? 0 : m_hp;
        }
    }

    public eCamp Camp = eCamp.None;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Init()
    {

    }

    public virtual void RemoveSelf()
    {

    }

    public virtual void OnHit(BulletMsg msg)
    {
        if (CurrentState == eActorState.Die)
            return;

        HP -= msg.damage;

        if (HP <= 0 && CurrentState != eActorState.Die)
        {
            SetState(eActorState.Die);
        }
    }

    public virtual void SetState(eActorState state)
    {
        //Debug.Log("state:" + state);
        LastState = CurrentState;
        CurrentState = state;

        if(m_animator!=null)
        m_animator.SetTrigger(state.ToString());

    }
}
