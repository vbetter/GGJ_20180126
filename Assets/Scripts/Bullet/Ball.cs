using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public eColor m_color = eColor.None;

    public BoxCollider2D m_BoxCollider2D;

    public Rigidbody2D m_Rigidbody2D;

    // Use this for initialization
    void Start () {
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


    }

	float m_lastY =0;
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.IsPause && !LevelManager.Instance.IsGameOver)
		{
			if(m_Rigidbody2D.velocity.y!=0)
			{
				if(transform.localPosition.y > m_lastY)
				{
					if(!Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Ball"),LayerMask.NameToLayer("Platform")))
					{
						Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"),LayerMask.NameToLayer("Platform"),true);
					}
				}
				else
				{
					if(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Ball"),LayerMask.NameToLayer("Platform")))
					{
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"), LayerMask.NameToLayer("Platform"), false);
                    }
                }
			}
			m_lastY = transform.localPosition.y	;

			//Debug.Log(m_Rigidbody2D.velocity.y);
		}
	}

	public void Init(eColor color)
	{
		m_color = color;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        GameObject go = collision.gameObject;
        string log = string.Format("go : {0} collision-> tag : {1}", go.name, go.tag);
        Debug.LogError(log);
        if (go.tag == "Enemy")
        {
            Debug.Log("score --");
        }
        else if(go.tag == "Boss")
        {
            Debug.Log("game over");
        }
        */

    }

}
