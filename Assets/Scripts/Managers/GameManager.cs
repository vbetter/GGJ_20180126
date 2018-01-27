using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Template.MonoSingleton<GameManager> {

    public int SelectPlayerCount = 1;//选择玩家人数

    bool m_isPause = false;
    public bool IsPause
    {
        get
        {
            return m_isPause;
        }
        set
        {
            if(value)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            m_isPause = value;
        }
    }

	// Use this for initialization
	void Start ()
    {
        
        StartCoroutine(EffectMgr.Instance.LoadData());
    }
	
	public void PauseGame()
	{
		IsPause = true;
	}

	public void ResumeGame()
	{
		IsPause = false;
	}

	public void ResetGame()
	{
		
	}
}
