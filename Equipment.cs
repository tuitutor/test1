using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[DisallowMultipleComponent]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public abstract class Equipment : MonoBehaviour, IHealthBonus, IManaBonus, ICombatBonus
{
    // used components. Assign in Inspector. Easier than GetComponent caching.
    public Health health;
    public Inventory inventory;

    [HideInInspector] // slots are created on start. don't modify manually.
    public List<ItemSlot> slots = new List<ItemSlot>();

    public int GetItemIndexByName(string itemName)
    {
        return slots.FindIndex(slot => slot.amount > 0 && slot.item.name == itemName);
    }

    // energy boni
    public int GetHealthBonus(int baseHealth)
    {
        return slots.Where(slot => slot.amount > 0).Sum(slot => ((EquipmentItem)slot.item.data).healthBonus);
    }
    public int GetHealthRecoveryBonus()
    {
        return 0;
    }
    public int GetManaBonus(int baseMana)
    {
        return slots.Where(slot => slot.amount > 0).Sum(slot => ((EquipmentItem)slot.item.data).manaBonus);
    }
    public int GetManaRecoveryBonus()
    {
        return 0;
    }

    // combat boni
    public int GetDamageBonus()
    {
        return slots.Where(slot => slot.amount > 0).Sum(slot => ((EquipmentItem)slot.item.data).damageBonus);
    }
    public int GetDefenseBonus()
    {
        return slots.Where(slot => slot.amount > 0).Sum(slot => ((EquipmentItem)slot.item.data).defenseBonus);
    }
}