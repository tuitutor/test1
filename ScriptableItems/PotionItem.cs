using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName="uRPG Item/Potion", order=999)]
public class PotionItem : UsableItem
{
    [Header("Potion")]
    public int usageHealth;
    public int usageMana;

    // usage
    public override Usability CanUse(PlayerInventory inventory, int inventoryIndex)
    {
        return Usability.Usable;
    }
    public override Usability CanUse(PlayerEquipment equipment, int hotbarIndex, Vector3 lookAt)
    {
        return Usability.Usable;
    }

    void ApplyEffects(GameObject player)
    {
        player.GetComponent<Health>().current += usageHealth;
        player.GetComponent<Mana>().current += usageMana;
    }

    public override void Use(PlayerInventory inventory, int inventoryIndex)
    {
        ApplyEffects(inventory.gameObject);

        // decrease amount
        ItemSlot slot = inventory.slots[inventoryIndex];
        slot.DecreaseAmount(1);
        inventory.slots[inventoryIndex] = slot;
    }
    public override void Use(PlayerEquipment equipment, int hotbarIndex, Vector3 lookAt)
    {
        ApplyEffects(equipment.gameObject);

        // decrease amount
        ItemSlot slot = equipment.slots[hotbarIndex];
        slot.DecreaseAmount(1);
        equipment.slots[hotbarIndex] = slot;
    }

    // tooltip
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{USAGEHEALTH}", usageHealth.ToString());
        tip.Replace("{USAGEMANA}", usageMana.ToString());
        return tip.ToString();
    }
}
