using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndButClicked : MonoBehaviour
{
    public GameObject canvasEND1;
    public GameObject canvas;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    public void ButClicked1()
    {
        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            //Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            //Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("END==============================");
//            canvas.SetActive(false);
            canvasEND1.SetActive(true);
            //            CheckEnds();
        }
    }

    public void ExitGame()
    {
        Debug.Log("EXIT==========================");
//        panel.SetActive(!panel.activeSelf);
//        canvasEND1.SetActive(true);
        GameStateManager.Quit();
//        Application.Quit();
    }
}
