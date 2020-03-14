// keep track of some meta info like class, account etc.
using UnityEngine;

public class Player : MonoBehaviour
{
    // the player GameObject as singleton, for easier access from other scripts
    public static GameObject player;
    [HideInInspector] public string className = ""; // the prefab name

    void Awake() { player = gameObject; }
}