using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkENDS : MonoBehaviour
{


    public GameObject PanelEND1;
    public GameObject PanelEND2;
    public GameObject PanelEND3;
    public GameObject PanelEND4;
    public GameObject PanelEND5;
    public GameObject PanelEND6;

    private void Start()
    {
        PanelEND1.SetActive(false);
        PanelEND2.SetActive(false);
        PanelEND3.SetActive(false);
        PanelEND4.SetActive(false);
        PanelEND5.SetActive(false);
        PanelEND6.SetActive(false);
        Debug.Log("//////////////////////////////");
        //       InitVar.Karma = InitVar.Karma + 1;
        //        InitVar.Health = InitVar.Health - 1;

        if (InitVar.Karma >= 13)
        {
            PanelEND1.SetActive(true);
            Debug.Log("22222222222222222222222222");
            //            GetComponent<Button>().interactable = false;
            //            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            //            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            //            Debug.Log("==============================");
            //            canvas.SetActive(false);
            //            CheckEnds();
        }
        else if (InitVar.Karma <= 0)
        {
            PanelEND2.SetActive(true);
            Debug.Log("3333333333333333");
        }
        else if (InitVar.Immune <= 2)
        {
            PanelEND3.SetActive(true);
            Debug.Log("444444444444444444444444");
        }
        else if (InitVar.Health>=12) 
        {
            PanelEND4.SetActive(true);
            Debug.Log("5555555555555555555555555");
        }
        else if (InitVar.Gold >= 3)
        {
            PanelEND5.SetActive(true);
            Debug.Log("666666666666666666666666");
        }
        else { PanelEND6.SetActive(true);
            Debug.Log("END-6==============");
        }
    }
    void checksENDS()
    {
        
        //       InitVar.Karma = InitVar.Karma + 1;
        //        InitVar.Health = InitVar.Health - 1;

        if (InitVar.Karma >= 13)
        {
            PanelEND1.SetActive(true);
            //            GetComponent<Button>().interactable = false;
            //            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            //            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            //            Debug.Log("==============================");
            //            canvas.SetActive(false);
            //            CheckEnds();
        }
        else if (InitVar.Karma <= 1)
                {
            PanelEND2.SetActive(true);
        }
        else { PanelEND1.SetActive(true); }
    }
}
