using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CreativeSpore.SmartColliders;
/// <summary>
/// 玩家类型
/// </summary>
public enum ePlayerType
{
    Programmer,
    Planner,
    Artist,
    Boss,//由玩家扮演的boss
    None,
}

/// <summary>
/// 阵营
/// </summary>
public enum eCamp
{
    None,
    Player,
    Enemy,
    Neutral,
}
public class Player : Actor
{
    public int PlayerNumber = 0;

    SmartPlatformController m_SmartPlatformController;
    PlayerControl m_PlayerControl;

    const string KeyFire1 = "Fire1";

    string m_Fire1 = "Fire1";

    protected Rigidbody2D m_Rigidbody2D;

    protected BoxCollider2D m_BoxCollider2D;

    protected Transform m_ballParent;

    SmartPlatformCollider m_SmartPlatformCollider;

    Ball m_ball = null;

    // Use this for initialization
    void Start () {

        m_ballParent = transform.Find("BallParent");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_SmartPlatformController = GetComponent<SmartPlatformController>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        SetState(eActorState.Idle);

        m_SmartPlatformCollider = GetComponent<SmartPlatformCollider>();
        m_SmartPlatformCollider.OnSideCollision = OnSideCollisionDelegate;
    }

    public void Init(int p)
    {
        PlayerNumber = p;

        m_SmartPlatformController = GetComponent<SmartPlatformController>();
        m_SmartPlatformController.Init(p);

        m_PlayerControl = GetComponent<PlayerControl>();
        m_PlayerControl.Init(p);
    }

    public void OnDie()
    {
        EffectMgr.Instance.CreateEffect(eEffectType.Boom, null, 1f, transform.localPosition);

        LevelManager.Instance.RemovePlayer(PlayerNumber);
    }

    public override void OnHit(BulletMsg msg)
    {
        if (CurrentState == eActorState.Coma)
            return;

        HP -= msg.damage;
        Debug.Log(string.Format("{0} OnHit ,HP:{1}",gameObject.name,HP));
        EffectMgr.Instance.CreateEffect(eEffectType.Boom, null, 1f, transform.localPosition);

        if (HP <= 0 && CurrentState != eActorState.Coma)
        {
            SetState(eActorState.Coma);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject go = collision.gameObject;
		string log = string.Format("go : {0} , tag : {1}",gameObject.name ,go.tag);
		Debug.Log(log);
		if (go.tag == "Ball")
		{
            if (gameObject.tag == "Boss")
            {
				//如果我是boss，并且我碰到了蛋，游戏结束
                Debug.Log("Game Over");
				LevelManager.Instance.GameOver();

            }
		}else if(go.tag =="Boss")
		{
			//如果我身上有蛋，游戏结束
			if(m_ball!=null)
			{
				Debug.Log("Game Over");

				LevelManager.Instance.GameOver();

			}
		}
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    void OnSideCollisionDelegate(SmartCollision2D collision, GameObject collidedObject)
    {
        if(collidedObject.tag == "Ball")
        Debug.Log("collidedObject:" + collidedObject.name);
    }

    /*
	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject go = collision.gameObject;
		string log = string.Format("go : {0} , tag : {1}",go.name ,go.tag);
		//Debug.LogError(log);
		if (go.tag == "Ball")
		{
			BulletMsg msg = new BulletMsg();
			msg.sender = gameObject;
			msg.damage = Power;
			Player player = go.GetComponent<Player>();
			player.OnHit(msg);
		}
	}
    */

    /// <summary>
    /// 捡球
    /// </summary>
    /// <param name="ball"></param>
    public void AddBall(Ball ball)
    {
        if (m_ballParent != null)
        {
            m_ball = ball;
            m_ball.m_Rigidbody2D.simulated = false;
            ball.transform.parent = m_ballParent;
            ball.transform.localPosition = Vector3.zero;
        }
    }

    public void PushBall()
    {
        if(m_ball!=null)
        {
            //手上有球
            m_ball.m_Rigidbody2D.simulated = true;
            m_ball.transform.parent = null;

            //向前方投掷
            Vector2 toPos = Vector2.zero;
            if(transform.localScale.x>0)
            {
                toPos = new Vector2(500f, 300f);
            }
            else if(transform.localScale.x < 0)
            {
                toPos = new Vector2(-500f, 300f);
            }
            m_ball.m_Rigidbody2D.AddForce(toPos);
            m_ball = null;
        }
        else
        {
            BoxCollider2D box = LevelManager.Instance.m_ball.m_BoxCollider2D;
            //手上无球
            if(box!=null)
            {
                if(m_BoxCollider2D.bounds.Intersects(box.bounds))
                {
                    AddBall(LevelManager.Instance.m_ball);
                }
            }
        }
    }
}
