using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBullt : MonoBehaviour {

    Vector3 m_start;

    Vector3 m_end;


    // Use this for initialization
    void Start () {
		
	}
	
    public void Init(Vector3 start , Vector3 end)
    {
        m_start = start;
        m_end = end;

        transform.position = m_start;
    }

	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.Instance.IsPause && !LevelManager.Instance.IsGameOver)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_end,  Time.deltaTime *3f);
        }

        if(m_end == transform.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        string log = string.Format("go : {0} , tag : {1}", go.name, go.tag);
        Debug.LogError(log);
        if (go.tag == "Player")
        {
            BulletMsg msg = new BulletMsg();
            msg.sender = gameObject;
            msg.damage = 1;
            Player player = go.GetComponent<Player>();
            player.OnHit(msg);

            Destroy(gameObject);
        }
    }
}
