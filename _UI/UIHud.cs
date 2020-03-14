using UnityEngine;
using UnityEngine.UI;

public class UIHud : MonoBehaviour
{
    public GameObject panel;
    public Slider healthSlider;
    public Text healthStatus;
    public Slider manaSlider;
    public Text manaStatus;
    public Slider enduranceSlider;
    public Text enduranceStatus;

    void Update()
    {
        GameObject player = Player.player;
        panel.SetActive(player != null); // hide while not in the game world
        if (!player) return;

        // health
        Health health = player.GetComponent<Health>();
        healthSlider.value = health.Percent();
        healthStatus.text = health.current + " / " + health.max;

        // mana
        Mana mana = player.GetComponent<Mana>();
        manaSlider.value = mana.Percent();
        manaStatus.text = mana.current + " / " + mana.max;

        // endurance
        Endurance endurance = player.GetComponent<Endurance>();
        enduranceSlider.value = endurance.Percent();
        enduranceStatus.text = endurance.current + " / " + endurance.max;
    }
}
