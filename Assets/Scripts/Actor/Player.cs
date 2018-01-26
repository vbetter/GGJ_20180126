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

    // Use this for initialization
    void Start () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_SmartPlatformController = GetComponent<SmartPlatformController>();

        SetState(eActorState.Idle);
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
		string log = string.Format("go : {0} , tag : {1}",go.name ,go.tag);
		//Debug.LogError(log);
		if (go.tag == "Ball")
		{
			/*
			BulletMsg msg = new BulletMsg();
			msg.sender = gameObject;
			msg.damage = Power;
			Player player = go.GetComponent<Player>();
			player.OnHit(msg);
			*/
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject go = collision.gameObject;
		string log = string.Format("go : {0} , tag : {1}",go.name ,go.tag);
		//Debug.LogError(log);
		if (go.tag == "Ball")
		{
			/*
			BulletMsg msg = new BulletMsg();
			msg.sender = gameObject;
			msg.damage = Power;
			Player player = go.GetComponent<Player>();
			player.OnHit(msg);
			*/
		}
	}
}
