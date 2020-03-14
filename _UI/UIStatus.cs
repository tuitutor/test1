// Note: this script has to be on an always-active UI parent, so that we can
// always react to the hotkey.
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthStatus;
    public Slider manaSlider;
    public Text manaStatus;
    public Slider enduranceSlider;
    public Text enduranceStatus;

    public Text levelText;
    public Text damageText;
    public Text defenseText;

    void Update()
    {
        GameObject player = Player.player;
        if (!player) return;

        Health health = player.GetComponent<Health>();
        healthSlider.value = health.Percent();
        healthStatus.text = health.current + " / " + health.max;

        Mana mana = player.GetComponent<Mana>();
        manaSlider.value = mana.Percent();
        manaStatus.text = mana.current + " / " + mana.max;

        Endurance endurance = player.GetComponent<Endurance>();
        enduranceSlider.value = endurance.Percent();
        enduranceStatus.text = endurance.current + " / " + endurance.max;

        levelText.text = player.GetComponent<Level>().current.ToString();
        damageText.text = player.GetComponent<Combat>().damage.ToString();
        defenseText.text = player.GetComponent<Combat>().defense.ToString();
    }
}
