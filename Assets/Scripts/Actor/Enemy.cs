using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SmartColliders;

public class Enemy : Actor
{
    [SerializeField]
    protected SpriteRenderer m_sp;

    public float m_interval = 1f;

    public float moveSpeed = 2f;
    public float maxSpeed = 5f;

    protected Transform frontCheck;

    protected Rigidbody2D m_Rigidbody2D;

    protected Transform groundCheck;
    protected bool grounded = false;

    protected SmartPlatformController m_SmartPlatformController;

    //检查目标点内是否有对象
    protected Transform Check_front;
    protected Transform Check_back;
    protected Transform Check_top;

    public bool m_headHasPlatform= false;//头上有跳板
    public bool m_frontHasWall = false;//前面是否有墙

    public bool HasAttack = false;//是否能够攻击

    private void Awake()
    {
        groundCheck = transform.Find("groundCheck");
        frontCheck = transform.Find("frontCheck").transform;
        Check_top = transform.Find("topCheck").transform;
        Check_back = transform.Find("backCheck").transform;

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_SmartPlatformController = GetComponent<SmartPlatformController>();
    }
    // Use this for initialization
    void Start () {

        //UpdateSnowState();

        SetState(eActorState.Idle);
    }
	
	// Update is called once per frame
	void Update () {
        //监测是否碰到地面
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1<<LayerMask.NameToLayer("Platform"));
        //监测头上有跳板
        m_headHasPlatform = Physics2D.Linecast(transform.position, Check_top.position, 1 << LayerMask.NameToLayer("Platform"));
        //监测前面是否有路
        m_frontHasWall = Physics2D.Linecast(transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Wall"));
       
    }

    public void DisableCurrentState()
    {
        m_animator.SetBool(CurrentState.ToString(), false);
    }


    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        string log = string.Format("go : {0} , tag : {1}",go.name ,go.tag);
        //Debug.LogError(log);
        if (go.tag == "Player")
        {
            if (CurrentState == eActorState.Idle || CurrentState == eActorState.Run)
            {
                //SetState(eActorState.Die);
                Debug.LogError("YouDie:" + go.name);
                Player player = go.GetComponent<Player>();
                player.OnDie();
            }
            else
            {



            }
        }
    }

    public void OnJump()
    {
        SetState(eActorState.Jump);
    }

    public void OnDie()
    {
        EffectMgr.Instance.CreateEffect(eEffectType.Birth, null, 1f, transform.localPosition);
        Destroy(gameObject);
    }

    public bool IsGround
    {
        get
        {
            return grounded;
        }
    }

    /// <summary>
    /// 是否满足攻击条件
    /// </summary>
    /// <returns></returns>
    public virtual bool EnableAttack()
    {
        return false;
    }

    public virtual void Attack()
    {

    }

    /// <summary>
    /// 获取最近的敌人
    /// </summary>
    /// <returns></returns>
    public Actor GetNearTarget()
    {
        float distance = 999;
        List<Player> players = LevelManager.Instance.AllPlayerList;
        Vector2 myPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Actor target = null;
        foreach (var item in players)
        {
            float dis = Vector2.Distance(new Vector2(item.transform.localPosition.x, item.transform.localPosition.y), myPos);
            if (dis <= distance && item.CurrentState != eActorState.Die && item.CurrentState != eActorState.Coma)
            {
                distance = dis;
                target = item;
            }
        }

        return target;
    }

   
}
