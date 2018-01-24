using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBehaviour : EnemyBehaviour
{
    float m_intervalTimer = 0;
    float m_interval = 1f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        m_intervalTimer = 0;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        m_intervalTimer = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("On Attack Update ");
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
}
