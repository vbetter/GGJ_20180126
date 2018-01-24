using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour {

    Vector3 m_dir = Vector3.zero;
    float m_speed = 1f;
	// Use this for initialization
	void Start ()
    {
        
	}
	
    public void Init(Vector3 dir,float speed =1)
    {
        m_dir = dir;
        m_speed = speed;
        Destroy(gameObject, 3f);
    }

	// Update is called once per frame
	void Update ()
    {
        if(!GameManager.Instance.IsPause)
        {
            transform.Translate(m_dir * Time.deltaTime * m_speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Actor actor = collision.GetComponent<Actor>();
            if(actor!=null)
            {
                BulletMsg msg = new BulletMsg();
                msg.damage = 1;
                msg.sender = gameObject;
                actor.OnHit(msg);
            }

            Remove();
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
