using UnityEngine;
using UnityEngine.UI;

public class UIBuffs : MonoBehaviour
{
    public UIBuffSlot slotPrefab;

    void Update()
    {
        GameObject player = Player.player;
        if (!player) return;

        PlayerSkills skills = player.GetComponent<PlayerSkills>();

        // instantiate/destroy enough slots
        UIUtils.BalancePrefabs(slotPrefab.gameObject, skills.buffs.Count, transform);

        // refresh all
        for (int i = 0; i < skills.buffs.Count; ++i)
        {
            UIBuffSlot slot = transform.GetChild(i).GetComponent<UIBuffSlot>();

            // refresh
            slot.image.color = Color.white;
            slot.image.sprite = skills.buffs[i].image;
            slot.tooltip.text = skills.buffs[i].ToolTip();
            slot.slider.maxValue = skills.buffs[i].buffTime;
            slot.slider.value = skills.buffs[i].BuffTimeRemaining();
        }
    }
}