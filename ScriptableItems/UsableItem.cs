// only usable items need usage functions
// we need bindings for inventory and equipment, so that inheriting templates
// can decide if they want to be usable from inventory, or only from equipment,
// etc.
// => using a sword from inventory would equip it.
//    using it from equipment would swing it.
// => some equipment items might need current lookAt point too, e.g. to fire a
//    weapon
using System.Text;
using UnityEngine;

// a bool is not enough for CanUse result, since each usable item can either be:
// - usable
// - cooldown (fire rate, potion cooldown, equipment )
// - empty (no ammo, empty water bottle, etc.)
// - never usable by this person or from this slot or whatever
public enum Usability : byte { Usable, Cooldown, Empty, Never }

public abstract class UsableItem : ScriptableItem
{
    [Header("Usage")]
    public bool keepUsingWhileButtonDown; // guns should keep shooting, buildings shouldn't be kept building while holding, etc.
    public AudioClip successfulUseSound; // swining axe at enemy etc.
    public AudioClip failedUseSound; // swinging axe but into the air etc.
    public AudioClip emptySound; // weapon 'clicking' if magazine is empty, etc.
    public float cooldown; // weapon fire rate, potion usage interval, etc.

    public bool shoulderLookAtWhileHolding;

    // CanUse checks for UI, Commands, etc.
    public abstract Usability CanUse(PlayerInventory inventory, int inventoryIndex);
    public abstract Usability CanUse(PlayerEquipment equipment, int equipmentIndex, Vector3 lookAt);

    // Use logic
    public abstract void Use(PlayerInventory inventory, int inventoryIndex);
    public abstract void Use(PlayerEquipment equipment, int equipmentIndex, Vector3 lookAt);

    // OnUse callback for effects, sounds, etc.
    // -> can't pass Inventory+slotIndex because .Use might clear it before getting here already
    // -> should always simulate a Use() again to decide which sounds to play etc.,
    //    so that we can simulate it for local player to avoid latency effects.
    //    (passing a 'result' bool wouldn't allow us to call OnUsed without Use() theN)
    public virtual void OnUsed(PlayerInventory inventory) {}
    public virtual void OnUsed(PlayerEquipment equipment, Vector3 lookAt) {}

    // tooltip
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{COOLDOWN}", cooldown.ToString());
        return tip.ToString();
    }
}
