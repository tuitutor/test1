using UnityEngine;
using UnityEngine.UI;

public class UISkillbar : MonoBehaviour
{
    public GameObject panel;
    public UISkillbarSlot slotPrefab;
    public Transform content;

    void Update()
    {
        GameObject player = Player.player;
        panel.SetActive(player != null); // hide while not in the game world
        if (!player) return;

        // get components
        Skillbar skillbar = player.GetComponent<Skillbar>();
        PlayerSkills skills = player.GetComponent<PlayerSkills>();
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        PlayerEquipment equipment = player.GetComponent<PlayerEquipment>();
        PlayerLook look = player.GetComponent<PlayerLook>();

        // instantiate/destroy enough slots
        UIUtils.BalancePrefabs(slotPrefab.gameObject, skillbar.slots.Length, content);

        // refresh all
        for (int i = 0; i < skillbar.slots.Length; ++i)
        {
            UISkillbarSlot slot = content.GetChild(i).GetComponent<UISkillbarSlot>();
            slot.dragAndDropable.name = i.ToString(); // drag and drop index

            // hotkey overlay (without 'Alpha' etc.)
            string pretty = skillbar.slots[i].hotKey.ToString().Replace("Alpha", "");
            slot.hotkeyText.text = pretty;

            // skill, inventory item or equipment item?
            int skillIndex = skills.GetSkillIndexByName(skillbar.slots[i].reference);
            int inventoryIndex = inventory.GetItemIndexByName(skillbar.slots[i].reference);
            int equipmentIndex = equipment.GetItemIndexByName(skillbar.slots[i].reference);
            if (skillIndex != -1)
            {
                Skill skill = skills.skills[skillIndex];

                bool canUse = skill.CanCast(player) && !look.IsFreeLooking();

                // hotkey pressed and not typing in any input right now?
                if (Input.GetKeyDown(skillbar.slots[i].hotKey) &&
                    !UIUtils.AnyInputActive() &&
                    canUse)
                {
                    skills.StartCast(skillIndex);
                }

                // refresh skill slot
                slot.button.interactable = canUse ;
                slot.button.onClick.SetListener(() => {
                    skills.StartCast(skillIndex);
                });
                slot.tooltip.enabled = true;
                slot.tooltip.text = skill.ToolTip();
                slot.dragAndDropable.dragable = true;
                slot.image.color = Color.white;
                slot.image.sprite = skill.image;
                float cooldown = skill.CooldownRemaining();
                slot.cooldownOverlay.SetActive(cooldown > 0);
                slot.cooldownText.text = cooldown.ToString("F0");
                slot.cooldownCircle.fillAmount = skill.cooldown > 0 ? cooldown / skill.cooldown : 0;
                slot.amountOverlay.SetActive(false);
            }
            else if (inventoryIndex != -1)
            {
                ItemSlot itemSlot = inventory.slots[inventoryIndex];

                // hotkey pressed and not typing in any input right now?
                if (Input.GetKeyDown(skillbar.slots[i].hotKey) && !UIUtils.AnyInputActive())
                    inventory.UseItem(inventoryIndex);

                // refresh inventory slot
                slot.button.onClick.SetListener(() => {
                    inventory.UseItem(inventoryIndex);
                });
                slot.tooltip.enabled = true;
                slot.tooltip.text = itemSlot.ToolTip();
                slot.dragAndDropable.dragable = true;
                slot.image.color = Color.white;
                slot.image.sprite = itemSlot.item.image;
                slot.cooldownOverlay.SetActive(false);
                slot.cooldownCircle.fillAmount = 0;
                slot.amountOverlay.SetActive(itemSlot.amount > 1);
                slot.amountText.text = itemSlot.amount.ToString();
            }
            else if (equipmentIndex != -1)
            {
                ItemSlot itemSlot = equipment.slots[equipmentIndex];

                // refresh equipment slot
                slot.button.onClick.RemoveAllListeners();
                slot.tooltip.enabled = true;
                slot.tooltip.text = itemSlot.ToolTip();
                slot.dragAndDropable.dragable = true;
                slot.image.color = Color.white;
                slot.image.sprite = itemSlot.item.image;
                slot.cooldownOverlay.SetActive(false);
                slot.cooldownCircle.fillAmount = 0;
                slot.amountOverlay.SetActive(itemSlot.amount > 1);
                slot.amountText.text = itemSlot.amount.ToString();
            }
            else
            {
                // clear the outdated reference
                skillbar.slots[i].reference = "";

                // refresh empty slot
                slot.button.onClick.RemoveAllListeners();
                slot.tooltip.enabled = false;
                slot.dragAndDropable.dragable = false;
                slot.image.color = Color.clear;
                slot.image.sprite = null;
                slot.cooldownOverlay.SetActive(false);
                slot.cooldownCircle.fillAmount = 0;
                slot.amountOverlay.SetActive(false);
            }
        }
    }
}
