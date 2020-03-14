// Finite State Machine
// -> we should react to every state and to every event for correctness
// -> we keep it functional for simplicity
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Health))]
public abstract class FiniteStateMachine : MonoBehaviour
{
    // Used components. Assign in Inspector. Easier than GetComponent caching.
    public Health health;

    // state
    public string state = "IDLE";

    // target for monsters etc.
    [HideInInspector] public GameObject target;

    void Start()
    {
        // change to dead if we spawned with 0 health
        if (health.current == 0) state = "DEAD";
    }

    // this function is called by the AggroArea (if any)
    public virtual void OnAggro(GameObject go) {}
}