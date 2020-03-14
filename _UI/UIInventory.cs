using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public UIInventorySlot slotPrefab;
    public Transform content;
    public Text goldText;

    void Update()
    {
        GameObject player = Player.player;
        if (!player) return;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        // instantiate/destroy enough slots
        UIUtils.BalancePrefabs(slotPrefab.gameObject, inventory.slots.Count, content);

        // refresh all items
        for (int i = 0; i < inventory.slots.Count; ++i)
        {
            UIInventorySlot slot = content.GetChild(i).GetComponent<UIInventorySlot>();
            slot.dragAndDropable.name = i.ToString(); // drag and drop index
            ItemSlot itemSlot = inventory.slots[i];

            if (itemSlot.amount > 0)
            {
                // refresh valid item
                int icopy = i; // needed for lambdas, otherwise i is Count
                slot.button.onClick.SetListener(() => {
                    if (itemSlot.item.data is UsableItem &&
                        ((UsableItem)itemSlot.item.data).CanUse(inventory, icopy) == Usability.Usable)
                        inventory.UseItem(icopy);
                });
                slot.tooltip.enabled = true;
                slot.tooltip.text = itemSlot.ToolTip();
                slot.dragAndDropable.dragable = true;
                slot.image.color = Color.white;
                slot.image.sprite = itemSlot.item.image;
                slot.amountOverlay.SetActive(itemSlot.amount > 1);
                slot.amountText.text = itemSlot.amount.ToString();
            }
            else
            {
                // refresh invalid item
                slot.button.onClick.RemoveAllListeners();
                slot.tooltip.enabled = false;
                slot.dragAndDropable.dragable = false;
                slot.image.color = Color.clear;
                slot.image.sprite = null;
                slot.amountOverlay.SetActive(false);
            }
        }

        // gold
        goldText.text = inventory.gold.ToString();
    }
}
