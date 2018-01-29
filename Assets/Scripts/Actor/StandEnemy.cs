using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandEnemy : Enemy {

    [SerializeField]
    GameObject m_cloneBullet;

    float m_attackTimer = 5;
    float m_attackMax = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(m_attackTimer<=m_attackMax)
        m_attackTimer += Time.deltaTime;

        if (m_attackTimer >= m_attackMax &&!GameManager.Instance.IsPause && !LevelManager.Instance.IsGameOver)
        {
            m_attackTimer = 0;
            Attack();
        }

    }

    public override void Attack()
    {
        GameObject go = Instantiate(m_cloneBullet);
        if (go != null)
        {
            go.transform.position = transform.position;
            go.transform.localScale = transform.localScale;

            SmallBullt bullect = go.GetComponent<SmallBullt>();
            if (bullect != null)
            {
                Vector3 endPos = new Vector3(transform.position.x +(transform.localScale.x  *  30f)
                    , transform.position.y, transform.position.z);

                bullect.Init(transform.position, endPos);
            }

			AudioManager.Instance.PlayGameEffectsMusic(Sound.p1);
        }
    }
}
