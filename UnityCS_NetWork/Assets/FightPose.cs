using UnityEngine;

public class FightPose : StateMachineBehaviour
{
    float d = 0f;

    // OnStateEnter
    override public void OnStateEnter(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.position
            += animator.gameObject.transform.forward * 0.2f;

    }

    // OnStateExit
    override public void OnStateExit(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.position
            -= animator.gameObject.transform.forward * 0.2f;
        animator.SetBool("punch", false);
        animator.SetBool("kick", false);
    }
}