using UnityEngine;
using UnityEngine.UI;

public class UICanvasMobButtonActivat : MonoBehaviour
{
    public Text text;
    public GameObject canvas; //Your target for the refference
    void Update()
    {
        // hide while not in the game world
        text.enabled = Player.player != null;
        canvas.SetActive(true);
    }
}
