using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    public GameObject panel;
    public Slider slider;
    public Text actionText;
    public Text progressText;

    bool CastInProgress(GameObject player, out float percentage, out string action, out string progress)
    {
        percentage = 0;
        action = "";
        progress = "";

        // get components
        PlayerSkills skills = player.GetComponent<PlayerSkills>();

        // currently casting?
        if (skills.current != -1)
        {
            Skill skill = skills.skills[skills.current];
            if (skill.CastTimeRemaining() > 0)
            {
                percentage = (skill.castTime - skill.CastTimeRemaining()) / skill.castTime;
                action = skill.name;
                progress = skill.CastTimeRemaining().ToString("F1") + "s";
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        GameObject player = Player.player;
        panel.SetActive(player != null); // hide while not in the game world
        if (!player) return;

        //  casting?
        if (CastInProgress(player, out float percentage, out string action, out string progress))
        {
            panel.SetActive(true);
            slider.value = percentage;
            actionText.text = action;
            progressText.text = progress;
        }
        // otherwise hide
        else panel.SetActive(false);
    }
}
