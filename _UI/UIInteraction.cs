using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    public GameObject panel;
    public Text hotkeyText;
    public Text actionText;

    void Update()
    {
        // looking at something interactable?
        GameObject player = Player.player;
        if (player != null)
        {
            PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
            if (interaction != null && interaction.current != null && interaction.current.IsInteractable())
            {
                panel.SetActive(true);
                hotkeyText.text = interaction.key.ToString();
                actionText.text = player.GetComponent<PlayerInteraction>().current.GetInteractionText();
            }
            else panel.SetActive(false);
        }
        else panel.SetActive(false);
    }
}
