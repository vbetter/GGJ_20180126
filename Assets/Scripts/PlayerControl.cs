using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    float m_interval = 0;
    float m_interval_Max = 0.5f;

	float m_interval2 = 0;
	float m_interval_Max2 = 5f;

    const string KeyFire1 = "Fire1";

    string m_Fire1 = "Fire1";

    int m_PlayerNumber = 0;

    [SerializeField]
    GameObject bomb;

    Player m_player;

	GameManager m_GameManager;
    // Use this for initialization
    void Start () {
        m_player = GetComponent<Player>();
		m_GameManager = GameManager.Instance;
    }

    public void Init(int p)
    {
        m_PlayerNumber = p;

        string pStr = string.Format("_p{0}", p);
        m_Fire1 = m_Fire1 + pStr;
    }

    // Update is called once per frame
    void Update ()
    {
        if(m_interval<= m_interval_Max)
        {
            m_interval += Time.deltaTime;
        }

		if(m_interval2<= m_interval_Max2)
		{
			m_interval2 += Time.deltaTime;
		}

		if(!m_GameManager.IsPause && !LevelManager.Instance.IsGameOver)
		{
			if ((Input.GetKey(Utils.GetKeyCodeByPlayer(KeyFire1, m_PlayerNumber))) && m_interval>= m_interval_Max)
			{
				if(m_player.IsController)
				{
					m_interval = 0;
					//Debug.Log(m_fireName);
					PlaySkill();
				}
			}

			//boss额外多一个技能
			if(m_player.PlayerType == ePlayerType.Boss)
			{
				if((Input.GetKey(Utils.GetKeyCodeByPlayer("Fire2", m_PlayerNumber))) && m_interval2>= m_interval_Max2)
				{
					m_interval2 = 0;
					StartCoroutine(m_player.Transmission());
				}
			}
		}

    }

    void PlaySkill()
    {
		if(m_player.PlayerType== ePlayerType.Boss)
		{
			m_player.ChangeColor();
		}
		else
		{
			//捡球、扔球
			m_player.PushBall();
		}

    }
}
