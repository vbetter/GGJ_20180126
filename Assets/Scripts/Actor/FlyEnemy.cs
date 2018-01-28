using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    TweenPosition m_TweenPosition;

    void Start()
    {
        m_TweenPosition = GetComponent<TweenPosition>();

        m_TweenPosition.OnPopEndEvent = OnMoveFinish;
    }

    public void OnMoveFinish()
    {
        Flip();
    }
}
