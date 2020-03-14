using UnityEngine;
using UnityEngine.UI;

public class UIEquipment : MonoBehaviour
{
    public UIEquipmentSlot slotPrefab;
    public Transform content;

    void Update()
    {
        GameObject player = Player.player;
        if (!player) return;
        PlayerEquipment equipment = player.GetComponent<PlayerEquipment>();

        // instantiate/destroy enough slots
        UIUtils.BalancePrefabs(slotPrefab.gameObject, equipment.slots.Count, content);

        // refresh all
        for (int i = 0; i < equipment.slots.Count; ++i)
        {
            UIEquipmentSlot slot = content.GetChild(i).GetComponent<UIEquipmentSlot>();
            slot.dragAndDropable.name = i.ToString(); // drag and drop slot
            ItemSlot itemSlot = equipment.slots[i];

            // set category overlay in any case. we use the last noun in the
            // category string, for example EquipmentWeaponBow => Bow
            // (disabled if no category, e.g. for archer shield slot)
            slot.categoryOverlay.SetActive(equipment.slotInfo[i].requiredCategory != "");
            string overlay = equipment.slotInfo[i].requiredCategory;
            slot.categoryText.text = overlay != "" ? overlay : "?";

            if (itemSlot.amount > 0)
            {
                // refresh valid item
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
                slot.tooltip.enabled = false;
                slot.dragAndDropable.dragable = false;
                slot.image.color = Color.clear;
                slot.image.sprite = null;
                slot.amountOverlay.SetActive(false);
            }
        }
    }
}
