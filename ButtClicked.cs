using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtClicked : MonoBehaviour
{
    public GameObject canvasEND;
    public GameObject canvasEND1;
    public GameObject canvas;
    public GameObject Sounds1;
    public AudioClip din; // source audio
    /// 
    /// 
    AudioSource audio;



    //============================================================================================/
    //===========================================ROOM1================================================/
    public void BlockButtons()

    {
        InitVar.Karma = InitVar.Karma + 1;
        InitVar.Health = InitVar.Health - 1;
        //Sounds1.SetActive(true);
        //Debug.Log("====Sounds1");

//        if (InitVar.RATquest == 1)
//        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("==============================");


        InitVar.RATquest = InitVar.RATquest = true;
            canvas.SetActive(false);
//            CheckEnds();
//        }

    }

    //public void PuskAudio()
    //{
    //    audio = GetComponent<AudioSource>();
    //    audio.PlayOneShot(din);
    //}
    public void BlockButtons1()
    {
        InitVar.Karma = InitVar.Karma - 1;
        InitVar.Immune = InitVar.Immune - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons2()
    {
        InitVar.Health = InitVar.Health - 1;
        InitVar.Immune = InitVar.Immune - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }

    public void BlockButtons3()

    {
        InitVar.Karma = InitVar.Karma + 1;
        InitVar.Gold = InitVar.Gold - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons4()
    {
        InitVar.Karma = InitVar.Karma - 1;
        InitVar.Immune = InitVar.Immune - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons5()
    {
        InitVar.Health = InitVar.Health - 1;
        InitVar.Immune = InitVar.Immune - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }

    //============================================================================================/
    //===ROOM2================================================/
    public void BlockButtons6()

    {
        InitVar.Health = InitVar.Health + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons7()
    {
        InitVar.Immune = InitVar.Immune - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons8()
    {
        InitVar.Gold = InitVar.Gold + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Gold);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons9()

    {
        InitVar.Karma = InitVar.Karma - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons10()
    {
        InitVar.Karma = InitVar.Karma - 1;
        InitVar.Health = InitVar.Health + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Health);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons11()
    {
        InitVar.Gold = InitVar.Gold + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Gold);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }



    //============================================================================================/
    //====ROOM3================================================/

    public void BlockButtons12()

    {
        InitVar.Karma = InitVar.Karma + 1;
        InitVar.Health = InitVar.Health + InitVar.Health/2;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons13()
    {
        InitVar.Karma = InitVar.Karma - 1;
        InitVar.Health = InitVar.Health * 2;
        InitVar.Immune = InitVar.Immune + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Health);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }

    //============================================================================================/
    //=====ROOM4================================================/

    public void BlockButtons14()

    {
        InitVar.Karma = InitVar.Karma - 1;
        InitVar.Health = InitVar.Health - 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons15()
    {
        InitVar.Karma = InitVar.Karma + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons16()
    {
//        InitVar.Health = InitVar.Health + 333;
//        InitVar.Immune = InitVar.Immune + 333;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }

    public void BlockButtons17()

    {
        InitVar.Gold = InitVar.Gold - 1;
        InitVar.Health = InitVar.Health + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Gold);
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons18()
    {
//        InitVar.Karma = InitVar.Karma + 222;
//        InitVar.Immune = InitVar.Immune + 222;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons19()
    {
        InitVar.Gold = InitVar.Gold - 1;
        InitVar.Immune = InitVar.Immune + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Gold);
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }

    //============================================================================================/
    //===ROOM5================================================/

    public void BlockButtons20()

    {
        InitVar.Karma = InitVar.Karma -1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons21()
    {
        InitVar.Karma = InitVar.Karma + 1;
        InitVar.Health = InitVar.Health + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }
    public void BlockButtons22()
    {
//        InitVar.Health = InitVar.Health + 333;
//        InitVar.Immune = InitVar.Immune + 333;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("3_BUTTON_CLICK=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
        }

    }

    public void BlockButtons23()

    {
        InitVar.Karma = InitVar.Karma + 1;
        InitVar.Health = InitVar.Health + InitVar.Health / 2;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Karma);
            Debug.Log("1_BUTTON_CLICK=" + InitVar.Health);
            Debug.Log("==============================");
            canvas.SetActive(false);
            CheckEnds();
        }

    }
    public void BlockButtons24()
    {
        InitVar.Karma = InitVar.Karma - 1;
        InitVar.Health = InitVar.Health * 2;
        InitVar.Immune = InitVar.Immune + 1;

        if (InitVar.isChecked == false)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("2_BUTTON_CLICK_karma=" + InitVar.Karma);
            Debug.Log("2_BUTTON_CLICK_health=" + InitVar.Health);
            Debug.Log("2_BUTTON_CLICK_immune=" + InitVar.Immune);
            Debug.Log("==============================");
            canvas.SetActive(false);
  //          canvasEND.SetActive(true);
            CheckEnds();

        }

    }
    //============================================================================================/
    //====THEEND================================================/
    public void CheckEnds()
    {
        //       Text test1;
        //        private int trest1;
        //        canvasEND.SetActive(true);
               canvasEND1.SetActive(true);
 //       canvasEND1.GameObject.Find("Canvas_ENDBOSS")
//            .GetComponent<Canvas>().gameObject
//            .SetActive(true);
//                GameObject canvasObject = 
//            GameObject.Find("Canvas_ENDBOSS").gameObject.SetActive(true);
        //        Transform textTr = canvasObject.transform.Find("Panel_END1");
        //        Text text = textTr.GetComponent<Text>();
        //        trest1 = gameObject.Find("Canvas_ENDBOSS/Panel_END1").GetComponent();
        //        test1.text = string.Format("{0:0,0.00}", test1);
        //        Text text = canvasEND.GetComponent<Transform>().Find("InputField").GetComponent<InputField>().GetComponent<Transform>().Find("Text").GetComponent<Text>();
        //       Debug.Log("text: " + text.text);
    }


}
