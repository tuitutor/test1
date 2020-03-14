using UnityEngine;

public class MountAnimation : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Mount mount;

    void Update()
    {
        // pass parameters to animation state machine
        // => passing the states directly is the most reliable way to avoid all
        //    kinds of glitches like movement sliding, attack twitching, etc.
        // => make sure to import all looping animations like idle/run/attack
        //    with 'loop time' enabled, otherwise the client might only play it
        //    once
        // => only play moving animation while the agent is actually moving. the
        //    MOVING state might be delayed to due latency or we might be in
        //    MOVING while a path is still pending, etc.

        // use owner's moving state for maximum precision (not if dead)
        animator.SetFloat("DirZ", 0);
        if (mount.owner != null)
        {
            PlayerMovement ownerMovement = mount.owner.GetComponent<PlayerMovement>();
            Vector3 localVelocity = mount.owner.transform.InverseTransformDirection(ownerMovement.controller.velocity);
            animator.SetFloat("DirZ", localVelocity.z);
            animator.SetBool("SWIMMING", ownerMovement.state == MoveState.MOUNTED_SWIMMING);
        }
        animator.SetBool("DEAD", mount.state == "DEAD");
    }
}
