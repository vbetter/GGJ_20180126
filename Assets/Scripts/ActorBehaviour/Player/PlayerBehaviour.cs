using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : BaseBehaviour
{

    protected Player m_self;//我自己

    protected Rigidbody2D m_Rigidbody2D;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_self = animator.GetComponent<Player>();
        m_Rigidbody2D = animator.GetComponent<Rigidbody2D>();

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        m_self.DisableCurrentState();
    }
}
