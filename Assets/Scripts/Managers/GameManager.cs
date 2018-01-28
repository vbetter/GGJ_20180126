using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Template.MonoSingleton<GameManager> {

    public int SelectPlayerCount = 2;//选择玩家人数

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

    bool m_isInit = false;

	// Use this for initialization
	void Start ()
    {
        m_isInit = true;
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
		IsPause = false;
		SelectPlayerCount = 2;
	}
		
}
