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
	
	// Update is called once per frame
	void Update () {
		
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
