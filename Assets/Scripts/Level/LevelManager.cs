using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Template.MonoSingleton<LevelManager>
{

    public int CurLevel = 1;

    [SerializeField]
    GameObject m_ClonePlanner, m_CloneProgrammer, m_CloneArtist, m_CloneBoss;

    List<Player> m_playerList = new List<Player>();
    public List<Player> AllPlayerList
    {
        get
        {
            return m_playerList;
        }
    }

    [SerializeField]
    GameObject m_CloneBall;

    public Ball m_ball = null;

	Dictionary <ePlayerType ,Vector3 > m_posDic = new Dictionary<ePlayerType, Vector3>();

    bool m_isInit = false;
    // Use this for initialization
    void Start()
    {
        if(m_isInit== false)
        {
            SceneManager.activeSceneChanged += OnSceneChanged;

            m_isInit = true;
        }

        //InitLevel();


    }

    void OnSceneChanged(Scene lastScene, Scene curScene)
    {
		if(curScene.name == "game")
		{
			GameObject ui =GameObject.Find("UIMainPanel");
			m_main = ui.GetComponent<UIMain>();

			if (curScene.name == "game")
			{
				StartCoroutine(InitLevel());
				print("game");
			}
			else if (curScene.name == "login")
			{
				print("login");
			}	
		}

    }

	/// <summary>


    /// <summary>
    /// 初始化关卡数据
    /// </summary>
    public IEnumerator InitLevel()
    {
		ClearAll();
        yield return new WaitForEndOfFrame();

		initPos();
		initBall();
        initPlayers();

        yield return null;
    }

	void initPos()
	{
       
		foreach (var item in PosArray) 
		{
			
			if(item.name =="p1")
			{
				m_posDic.Add(ePlayerType.Programmer,item.transform.localPosition);
			}else if(item.name =="p2")
			{
				m_posDic.Add(ePlayerType.Planner,item.transform.localPosition);
			}
			else if(item.name =="p3")
			{
				m_posDic.Add(ePlayerType.Artist,item.transform.localPosition);
			}
			else if(item.name =="p4")
			{
				m_posDic.Add(ePlayerType.Boss,item.transform.localPosition);
			}
		}
	}

	public void ClearAll()
	{
        currentTimer = MaxTime;
        m_posArray = null;
        IsGameOver = false;
		m_posDic.Clear();

        ClearBall();
		ClearPlayers();
		ClearEnemies();
	}

	void ClearBall()
	{
		if(m_ball!=null)
		{
			GameObject.Destroy(m_ball.gameObject);
			m_ball = null;
		}
	}

	void ClearEnemies()
	{
		
	}

	void ClearPlayers()
	{
		if(AllPlayerList!=null && AllPlayerList.Count>0)
		{
			for (int i = AllPlayerList.Count-1; i >=0; i--) 
			{
				Player player = AllPlayerList[i];
				GameObject.Destroy(player.gameObject);
                AllPlayerList.RemoveAt(i);

            }


		}
	}

    void initBall()
    {
        GameObject go = Instantiate(m_CloneBall);
        if(go!=null)
        {
            m_ball = go.GetComponent<Ball>();
            m_ball.transform.position = new Vector3(0, -5.3f, 0);
        }
    }

    public void ClearPlayrs()
    {
        if (m_playerList.Count > 0)
        {
            for (int i = 0; i < m_playerList.Count; i++)
            {
                Player player = m_playerList[i];
                player.RemoveSelf();
                m_playerList.Remove(player);
                Destroy(player.gameObject);
            }
        }
    }

	public void RemovePlayer(ePlayerType index)
    {
        for (int i = 0; i < m_playerList.Count; i++)
        {
            Player player = m_playerList[i];
            if (player.PlayerType == index)
            {
                player.RemoveSelf();
                m_playerList.Remove(player);
                Destroy(player.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// 初始化玩家
    /// </summary>
    void initPlayers()
    {
        int count = GameManager.Instance.SelectPlayerCount;

		if (PosArray == null || PosArray.Length < count)
        {
            Debug.LogError("BirthPosition init Error");
            return;
        }

        if (count >= 2 && count <= 4)
        {
            for (int i = 0; i < count; i++)
            {
				ePlayerType playerType = (ePlayerType)i;

				if(i==count-1)
				{
					//必须有一个boss
					playerType  = ePlayerType.Boss;
				}

				GameObject go = CreatePlayer(playerType);
                if (go != null)
                {
					go.transform.localPosition = GetBirtPositionByPlayerType(playerType);
                    Player player = go.GetComponent<Player>();
					player.Init(playerType,i+1);
                    m_playerList.Add(player);
                    EffectMgr.Instance.CreateEffect(eEffectType.Birth, null, 1f, new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, 0f));
                }
                else
                {
                    Debug.LogError("CreatePlayer is null!");
                }
            }
        }
    }

    GameObject CreatePlayer(ePlayerType type)
    {
        GameObject go = null;
        switch (type)
        {
            case ePlayerType.None:
                break;
            case ePlayerType.Programmer:
                go = Instantiate(m_CloneProgrammer);
                break;
            case ePlayerType.Planner:
                go = Instantiate(m_ClonePlanner);
                break;
            case ePlayerType.Artist:
                go = Instantiate(m_CloneArtist);
                break;
            case ePlayerType.Boss:
                go = Instantiate(m_CloneBoss);
                break;
            default:
                break;
        }

        return go;
    }

	GameObject[] m_posArray = null;
	GameObject[] PosArray

	{
		get
		{
			if(m_posArray==null)
			{
				m_posArray = GameObject.FindGameObjectsWithTag("BirthPosition");
			}
			return m_posArray;
		}
	}
    
	Vector3 GetBirtPositionByPlayerType(ePlayerType type)
	{
		Vector3 pos = Vector3.zero;

		m_posDic.TryGetValue(type,out pos);

		return pos;
	}

	#region 暂停/恢复
	[SerializeField]
	UIMain m_main;

	public bool IsGameOver =false;

    /// <summary>
    /// value == true,protecters vectory
    /// value == false,catchers vectory
    /// </summary>
    /// <param name="value"></param>
	public void GameOver(bool value)
	{
		if(IsGameOver == false)
		{
			GameManager.Instance.IsPause = true;
			IsGameOver = true;
            if(value)
            {
                m_main.ShowProtectersVectory(true);
            }
            else
            {
                m_main.ShowCatchersVectory(true);
            }
		}
	}


	public void PauseGame()
	{
		m_main.ShowPauseUI(true);
	}

	public void ResumeGame()
	{
		m_main.ShowPauseUI(false);
	}

    public void ReplayGame()
    {
        ClearAll();
        GameManager.Instance.ResetGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

	#endregion



	#region 倒计时

	public float currentTimer =0f;
	public float MaxTime = 60f;



	#endregion


	public Vector3 GetPlayerPositionByColor(eColor color)
	{
		foreach (var item in AllPlayerList) {
			if(item.PlayerType!= ePlayerType.Boss && item.Color == color)
			{
				return item.transform.localPosition;
			}
		}
		return Vector3.zero;
	}

    public bool IsHaveColorByPlayer(eColor color)
    {
        foreach (var item in AllPlayerList)
        {
            if (item.PlayerType != ePlayerType.Boss && item.Color == color)
            {
                return true;
            }
        }
        return false;
    }
}
