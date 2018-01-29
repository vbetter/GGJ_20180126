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
	public ePlayerType PlayerType = ePlayerType.None;

    SmartPlatformController m_SmartPlatformController;
    PlayerControl m_PlayerControl;

    const string KeyFire1 = "Fire1";

    string m_Fire1 = "Fire1";

    protected Rigidbody2D m_Rigidbody2D;

    protected BoxCollider2D m_BoxCollider2D;

    protected Transform m_ballParent;

    SmartPlatformCollider m_SmartPlatformCollider;

    Ball m_ball = null;

    [SerializeField]
    SpriteRenderer m_sprite;

    [SerializeField]
    Sprite m_green, m_blue, m_yellow, m_red;

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

	public void Init(ePlayerType type,int psIndex)
    {
		PlayerType = type;

        m_SmartPlatformController = GetComponent<SmartPlatformController>();
		m_SmartPlatformController.Init(psIndex);

        m_PlayerControl = GetComponent<PlayerControl>();
		m_PlayerControl.Init(psIndex);

        if(PlayerType == ePlayerType.Boss)
        {
            StartCoroutine(ShowTalk(0));
        }

        initByPlayerType(type);
    }

    void initByPlayerType(ePlayerType type)
    {
        switch (type)
        {
            case ePlayerType.Programmer:
                Color = eColor.Blue;
                break;
            case ePlayerType.Planner:
                Color = eColor.Yellow;
                break;
            case ePlayerType.Artist:
                Color = eColor.Red;
                break;
            case ePlayerType.Boss:
                Color = eColor.None;
                break;
            case ePlayerType.None:
                break;
            default:
                break;
        }
    }

    public void OnDie()
    {
        EffectMgr.Instance.CreateEffect(eEffectType.Boom, null, 1f, transform.localPosition);

        LevelManager.Instance.RemovePlayer(PlayerType);
    }

    public override void OnHit(BulletMsg msg)
    {
        //蛋会掉落
        if (m_ball != null)
        {
            //手上有球
            m_ball.m_Rigidbody2D.simulated = true;
            m_ball.transform.parent = null;
            m_ball = null;
        }

        //玩家跌落
        if(CurrentState!= eActorState.Drop)
        {
            CurrentState = eActorState.Drop;

            m_SmartPlatformCollider.EnableCollision2D = false;
            m_SmartPlatformCollider.EnableCollision3D = false;
        }

		AudioManager.Instance.PlayGameEffectsMusic(Sound.d1);

    }

    private void Update()
    {
        if(CurrentState == eActorState.Drop && !GameManager.Instance.IsPause && !LevelManager.Instance.IsGameOver)
        {
            if(transform.localPosition.y<= -4.5f)
            {
                CurrentState = eActorState.Idle;

                m_SmartPlatformCollider.EnableCollision2D = true;
                m_SmartPlatformCollider.EnableCollision3D = true;
            }

            if(m_isTalking && m_talkRender!=null)
            {
                m_talkRender.transform.localScale = new Vector3(transform.localScale.x* m_talkRender.transform.localScale.x, m_talkRender.transform.localScale.y, m_talkRender.transform.localScale.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject go = collision.gameObject;
		//string log = string.Format("go : {0} , tag : {1}",gameObject.name ,go.tag);
		//Debug.Log(log);
		if (go.tag == "Ball")
		{
            if (gameObject.tag == "Boss")
            {
				//如果我是boss，并且我碰到了蛋，游戏结束
                Debug.Log("Game Over");
				LevelManager.Instance.GameOver(false);

            }
		}else if(go.tag =="Boss")
		{
			//如果我身上有蛋，游戏结束
			if(m_ball!=null)
			{
				Debug.Log("Game Over");

				LevelManager.Instance.GameOver(false);

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
                toPos = new Vector2(600f, 500f);
            }
            else if(transform.localScale.x < 0)
            {
                toPos = new Vector2(-600f, 500f);
            }
            m_ball.m_Rigidbody2D.AddForce(toPos);
            m_ball = null;
        }
        else
        {
			if(LevelManager.Instance.m_ball==null)
				return;
			
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

	/// <summary>
	/// 改变颜色
	/// </summary>
	public void ChangeColor()
	{
        if (PlayerType != ePlayerType.Boss)
            return;

		int color =(int)Color ;
		color++;
		if(color>(int)eColor.None)
		{
			color =0;
		}
		Color = (eColor)color ;
		Debug.Log("ChangeColor:"+Color);

        //改变颜色
        switch (Color)
        {
            case eColor.Blue:
                m_sprite.sprite = m_blue;
                break;
            case eColor.Red:
                m_sprite.sprite = m_red;
                break;
            case eColor.Yellow:
                m_sprite.sprite = m_yellow;
                break;
            case eColor.None:
                m_sprite.sprite = m_green;
                break;
            default:
                break;
        }
    }


	bool m_isTransmissionING = false;
	/// <summary>
	/// 传送
	/// </summary>
	public IEnumerator Transmission()
	{
		//如果正在传送
		if(m_isTransmissionING )
        {
            StartCoroutine(ShowTalk(1));
            yield break;
        }
			
		
		if(Color == eColor.None || LevelManager.Instance.IsHaveColorByPlayer(Color) == false)
        {
            StartCoroutine(ShowTalk(1));
            yield break;
        }
		
		Vector3 pos = LevelManager.Instance.GetPlayerPositionByColor(Color);
		if(pos!=Vector3.zero)
		{
            Vector3 offset = new Vector3(0, -1f, 0f);
            EffectMgr.Instance.CreateEffect(eEffectType.ManaRegenerationBlue, null, 3f, transform.localPosition + offset);
            EffectMgr.Instance.CreateEffect(eEffectType.ShieldEffectSubtleGold, null, 3f, pos+ offset);

            m_SmartPlatformCollider.EnableCollision2D = false;
			m_SmartPlatformCollider.EnableCollision3D = false;
			m_Rigidbody2D.simulated = false;
			m_isTransmissionING = true;
			yield return new WaitForSeconds(3f);
			Debug.Log("Transmission pos:"+pos);
			transform.localPosition = new Vector3(pos.x,pos.y+0.5f,pos.z);

			yield return new WaitForSeconds(1f);
			m_isTransmissionING= false;
			m_SmartPlatformCollider.EnableCollision2D = true;
			m_SmartPlatformCollider.EnableCollision3D = true;
			m_Rigidbody2D.simulated = true;
		}

		//Debug.Log("Transmission");

		yield return null;
	}

	public bool IsController
	{
		get
		{
			if(m_isTransmissionING)
				return false;
			
			return true;
		}
	}

    [SerializeField]
    Sprite[] m_talkArray;

    [SerializeField]
    SpriteRenderer m_talkRender;

    bool m_isTalking = false;

    IEnumerator ShowTalk(int random =0)
    {
        if(m_isTalking ==false)
        {
            if (random == 0) random = Random.Range(0, m_talkArray.Length);

            if (random < m_talkArray.Length)
            {
                m_talkRender.sprite = m_talkArray[random];
                m_talkRender.gameObject.SetActive(true);
            }
            m_isTalking = true;

            yield return new WaitForSeconds(2f);
            m_talkRender.gameObject.SetActive(false);
            m_isTalking = false;
        }

        yield return null;
    }
}
