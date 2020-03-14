using UnityEngine;
using UnityEngine.UI;

public class UIExperienceBar : MonoBehaviour
{
    public GameObject panel;
    public Slider slider;
    public Text statusText;

    void Update()
    {
        GameObject player = Player.player;
        panel.SetActive(player != null); // hide while not in the game world
        if (!player) return;

        Experience experience = player.GetComponent<Experience>();
        Level level = player.GetComponent<Level>();

        slider.value = experience.Percent();
        statusText.text = "Lv." + level.current + " (" + (experience.Percent() * 100).ToString("F2") + "%)";
    }
}
