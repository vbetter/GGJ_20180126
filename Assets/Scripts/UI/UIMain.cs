using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour {

	[SerializeField]
	UISlider m_sider;

	[SerializeField]
	GameObject m_resetBtn;

	[SerializeField]
	GameObject m_pauseUI;

	LevelManager m_LevelManager;

    [SerializeField]
    UILabel m_title;

    [SerializeField]
    GameObject m_CatchersVectory;

    [SerializeField]
    GameObject m_ProtectersVectory;

    // Use this for initialization
    void Start () {
        m_CatchersVectory.SetActive(false);
        m_ProtectersVectory.SetActive(false);

		m_LevelManager = LevelManager.Instance;

        AudioManager.Instance.PlayBackgroundMusic(Sound.BG_Fight);
    }

    public void ResetUI()
    {
        m_CatchersVectory.SetActive(false);
        m_ProtectersVectory.SetActive(false);

        AudioManager.Instance.PlayBackgroundMusic(Sound.BG_Fight);

    }

    public void ShowProtectersVectory(bool value)
    {
        if(value!= m_ProtectersVectory.activeSelf)
        m_ProtectersVectory.SetActive(true);
    }

    public void ShowCatchersVectory(bool value)
    {
        if (value != m_ProtectersVectory.activeSelf)
            m_CatchersVectory.SetActive(true);
    }

	public void ShowPauseUI(bool value)
	{
		if(m_pauseUI.activeSelf!=value)
		m_pauseUI.SetActive(value);
	}


	// Update is called once per frame
	void Update () 
	{
		if(!GameManager.Instance.IsPause && !m_LevelManager.IsGameOver)
		{
			m_LevelManager.currentTimer-=Time.deltaTime;
			//float p = m_LevelManager.currentTimer/m_LevelManager.MaxTime;
			//m_sider.value = p;

            m_title.text = string.Format("Protect eggs for {0} seconds",LevelManager.Instance.currentTimer.ToString("f1"));


            if (m_LevelManager.currentTimer<=0)
			{
				m_LevelManager.GameOver(true);
			}
		}

		if (Input.GetKeyDown(KeyCode.Joystick1Button12)
			|| Input.GetKeyDown(KeyCode.Joystick2Button12)
			|| Input.GetKeyDown(KeyCode.Joystick3Button12)
		)
		{
            if (LevelManager.Instance.IsGameOver)
            {
                LevelManager.Instance.ReplayGame();
                return;
            }
            else
            {
                if (GameManager.Instance.IsPause)
                {
                    LevelManager.Instance.ClearAll();
                    GameManager.Instance.ResetGame();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("login");
                }
            }
              
		}

		if (Input.GetKeyDown(KeyCode.Joystick1Button9)
			|| Input.GetKeyDown(KeyCode.Joystick2Button9)
			|| Input.GetKeyDown(KeyCode.Joystick3Button9)
		)
		{
			if(LevelManager.Instance.IsGameOver)
			{
				LevelManager.Instance.ClearAll();
				GameManager.Instance.ResetGame();
				UnityEngine.SceneManagement.SceneManager.LoadScene("login");

            }
            else
            {
                if (!GameManager.Instance.IsPause)
                {
                    GameManager.Instance.PauseGame();
                    LevelManager.Instance.PauseGame();
                }
                else
                {
                    GameManager.Instance.ResumeGame();
                    LevelManager.Instance.ResumeGame();
                }
            }
		}
	}
}
