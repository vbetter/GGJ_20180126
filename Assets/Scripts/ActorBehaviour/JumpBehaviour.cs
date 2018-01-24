using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : EnemyBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        m_Rigidbody2D.AddForce(new Vector2(0, m_self.JumpPower));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if(m_stayTime>2f)
        {
            m_self.SetState(eActorState.Idle);
        }
    }
}
