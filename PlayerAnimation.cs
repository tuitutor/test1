﻿using UnityEngine;
using StandardAssets.Characters.Physics;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Components")]
    public Health health;
    public OpenCharacterController controller;
    public PlayerMovement movement;
    public PlayerEquipment equipment;
    public PlayerMountUsage mountUsage;

    [Header("Dampening")]
    public float directionDampening = 0.05f;
    public float turnDampening = 0.1f;

    // helpers
    Vector3 lastForward;

    void Start()
    {
        lastForward = transform.forward;
    }

    // Vector.Angle and Quaternion.FromToRotation and Quaternion.Angle all end
    // up clamping the .eulerAngles.y between 0 and 360, so the first overflow
    // angle from 360->0 would result in a negative value (even though we added
    // something to it), causing a rapid twitch between left and right turn
    // animations.
    //
    // the solution is to use the delta quaternion rotation.
    // when turning by 0.5, it is:
    //   0.5 when turning right (0 + angle)
    //   364.6 when turning left (360 - angle)
    // so if we assume that anything >180 is negative then that works great.
    static float AnimationDeltaUnclamped(Vector3 lastForward, Vector3 currentForward)
    {
        Quaternion rotationDelta = Quaternion.FromToRotation(lastForward, currentForward);
        float turnAngle = rotationDelta.eulerAngles.y;
        return turnAngle >= 180 ? turnAngle - 360 : turnAngle;
    }

    void Update()
    {
        // local velocity (based on rotation) for animations
        Vector3 localVelocity = transform.InverseTransformDirection(controller.velocity);

        // Turn value so that mouse-rotating the character plays some animation
        // instead of only raw rotating the model.
        float turnAngle = AnimationDeltaUnclamped(lastForward, transform.forward);
        lastForward = transform.forward;

        // apply animation parameters to all animators.
        // there might be multiple if we use skinned mesh equipment.
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.SetBool("DEAD", health.current == 0);
            animator.SetFloat("DirX", localVelocity.x, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("DirY", localVelocity.y, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("DirZ", localVelocity.z, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("Turn", turnAngle, turnDampening, Time.deltaTime); // smooth turn
            animator.SetBool("CROUCHING", movement.state == MoveState.CROUCHING);
            animator.SetBool("CRAWLING", movement.state == MoveState.CRAWLING);
            animator.SetBool("CLIMBING", movement.state == MoveState.CLIMBING);
            animator.SetBool("SWIMMING", movement.state == MoveState.SWIMMING);
            animator.SetBool("MOUNTED", mountUsage.IsMounted()); // easier than MOUNTED, MOUNTED_JUMPING etc. state checks
            // smoothest way to do climbing-idle is to stop right where we were
            if (movement.state == MoveState.CLIMBING)
                animator.speed = localVelocity.y == 0 ? 0 : 1;
            else
                animator.speed = 1;

            // grounded detection works best via .state
            // -> check AIRBORNE state instead of controller.isGrounded to have some
            //    minimum fall tolerance so we don't play the AIRBORNE animation
            //    while walking down steps etc.
            animator.SetBool("OnGround", movement.state != MoveState.AIRBORNE || mountUsage.IsMounted());
            if (controller.isGrounded) animator.SetFloat("JumpLeg", movement.jumpLeg);

            // upper body layer
            // note: UPPERBODY_USED is fired from PlayerHotbar.OnUsedItem
            // -> find right hand equipment slot first
            int slotIndex = equipment.GetEquipmentTypeIndex("RightHand");
            animator.SetBool("UPPERBODY_HANDS", equipment.slots[slotIndex].amount == 0);
            // -> tool parameters are all set to false and then the current tool is
            //    set to true
            animator.SetBool("UPPERBODY_CROSSBOW", false);
            animator.SetBool("UPPERBODY_AXE", false);
            if (movement.state != MoveState.CLIMBING && // not while climbing
                equipment.slots[slotIndex].amount > 0 &&
                equipment.slots[slotIndex].item.data is WeaponItem)
            {
                WeaponItem weapon = (WeaponItem)equipment.slots[slotIndex].item.data;
                if (!string.IsNullOrWhiteSpace(weapon.upperBodyAnimationParameter))
                    animator.SetBool(weapon.upperBodyAnimationParameter, true);
            }
        }
    }

    public void OnSkillCastStarted(Skill skill)
    {
        // apply animation parameters to all animators.
        // there might be multiple if we use skinned mesh equipment.
        foreach (Animator animator in GetComponentsInChildren<Animator>())
            animator.SetTrigger("UPPERBODY_CASTING");
    }
}
