using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour {

	[SerializeField]
	UISlider m_sider;

	[SerializeField]
	GameObject m_fails;

	[SerializeField]
	GameObject m_resetBtn;

	[SerializeField]
	GameObject m_pauseUI;

	LevelManager m_LevelManager;

	// Use this for initialization
	void Start () {
		m_fails.SetActive(false);

		UIEventListener.Get(m_resetBtn).onClick = OnClickStartGame;

		m_LevelManager = LevelManager.Instance;
	}

	public void ShowGameOver()
	{
		Debug.Log("UI Game Over");
		m_fails.SetActive(true);
	}

	public void ShowStartGame()
	{
		m_fails.SetActive(false);
	}

	void OnClickStartGame(GameObject go)
	{
		
	}

	public void ShowPauseUI(bool value)
	{
		m_pauseUI.SetActive(value);
	}


	// Update is called once per frame
	void Update () 
	{
		if(!GameManager.Instance.IsPause && !m_LevelManager.m_isGameOver)
		{
			m_LevelManager.currentTimer+=Time.deltaTime;
			float p = m_LevelManager.currentTimer/m_LevelManager.MaxTime;
			m_sider.value = p;

			if(m_LevelManager.currentTimer>=m_LevelManager.MaxTime)
			{
				m_LevelManager.GameOver();
			}
		}

		if (Input.GetKeyDown(KeyCode.Joystick1Button12)
			|| Input.GetKeyDown(KeyCode.Joystick2Button12)
			|| Input.GetKeyDown(KeyCode.Joystick3Button12)
		)
		{
			if(!GameManager.Instance.IsPause)
			{
				GameManager.Instance.PauseGame();
				LevelManager.Instance.PauseGame();
			}
			else{
				GameManager.Instance.ResumeGame();
				LevelManager.Instance.ResumeGame();
			}
		}
	}
}
