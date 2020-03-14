﻿// Interact with Interactables
// -> IsReachable checks so we can't interact through walls. This works way better
//    than a simple raycast because for example, if we crouch in front of an
//    obstacle and we can see an item that we couldn't naturally pick, IsReachable
//    usually returns false too.
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Inventory))]
public class PlayerInteraction : MonoBehaviour
{
    // Used components. Assign in Inspector. Easier than GetComponent caching.
    [Header("Components")]
    public Health health;
    public PlayerLook look;

    // item drop looting
    [Header("Interaction")]
    public float range = 3;
    public KeyCode key = KeyCode.F;

    // interactable that we currently look at. save here so we don't have to
    // raycast again if we need it in UI etc.
    [HideInInspector] public Interactable current;

    // raycast into 'direction', from eyes, with max distance, to check if we
    // look at an interactable. can be used on client to find out what we look
    // at, and on server to find out if the door etc. is actually reachable.
    // (from eyes, not from camera, so that we can't pick items behind a wall
    //  even if a camera is above the player can see the item/door/etc.)
    Interactable RaycastFindInteractable(Vector3 direction)
    {
        // raycast against ALL layers so that we don't hit an interactable if
        // anything is in front of it, e.g. a wall. otherwise we could pick
        // items through walls, etc.
        // (ignore self in raycasts, otherwise animation might cause cast hits)
        RaycastHit hit;
        if (Utils.RaycastWithout(look.headPosition, direction, out hit, range, gameObject))
            return hit.transform.GetComponent<Interactable>();
        return null;
    }

    public void Interact(Vector3 lookAt)
    {
        // validate: alive?
        if (health.current > 0)
        {
            // direction := look at position - eyes
            Vector3 direction = lookAt - look.headPosition;

            // raycast to make sure that we can't pick up items through walls,
            // etc.
            Interactable interactable = RaycastFindInteractable(direction);
            if (interactable != null)
                interactable.OnInteract(gameObject);
        }
    }

    void Update()
    {
        // raycast into the scene to check if we are looking at interactables
        // only if not over a UI element & not pinching on mobile
        // note: this only works if the UI's CanvasGroup blocks Raycasts
        if (!Utils.IsCursorOverUserInterface() && Input.touchCount <= 1)
        {
            // always save in 'current' for interactable UI etc.
            Vector3 direction = look.lookDirectionRaycasted;
            current = RaycastFindInteractable(direction);

            // interactable and pressing the interact key?
            if (current != null && Input.GetKeyDown(key))
            {
                // interact
                Interact(look.lookPositionRaycasted);
            }
        }
    }
}