using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TriggerzoneRat_v1 : MonoBehaviour
{
    //   public Level level;

    //    public int baseRecoveryPerTick = 0;
    //   public LevelBasedInt baseHealth = new LevelBasedInt { baseValue = 100 };

//    public int Karma;

//    public int Karma_back;
//    public int Immune_back;
//    public int Health_back;
//    public int Gold_back;

    public GameObject canvas; //Your target for the refference

    public GameObject charcter;
    private bool isLocked;

    public void OnTriggerEnter(Collider other)
    {
        if (canvas.tag == "RAT" )
        {
            if (InitVar.RATquest == true)
            {
                Debug.Log("Box TRUE");
                canvas.SetActive(false);
            }
                 
        }else
        charcter.SetActive(false);
        {
            Debug.Log("Box False!");
            canvas.SetActive(true);

        }
        //else if(canvas.tag == "POORMAN")
        //{
        //    Debug.Log("Box POORMAN");
        //    InitVar.Gold = InitVar.Gold + 1;
        //}
        //else if (canvas.tag == "GOLDTABLE")
        //{
        //    Debug.Log("Box GOLDTABLE");
        //    InitVar.Immune = InitVar.Immune + 1;
        //}
        //else if (canvas.tag == "BUDDHA")
        //{
        //    Debug.Log("Box BUDDHA");
        //    InitVar.Health = InitVar.Health + 1;
        //}
        //else if (canvas.tag == "BOSSMIDDLE")
        //{
        //    Debug.Log("Box BOSSMIDDLE+2");
        //    InitVar.Karma = InitVar.Karma + 2;
        //}
        //else if (canvas.tag == "APPLE")
        //{
        //    Debug.Log("Box APPLE+2");
        //    InitVar.Immune = InitVar.Immune + 2;
        //}
        //else if (canvas.tag == "GNOME")
        //{
        //    Debug.Log("Box GNOME+2");
        //    InitVar.Gold = InitVar.Gold + 2;
        //}
        //else if (canvas.tag == "ANGEL")
        //{
        //    Debug.Log("Box ANGEL+2");
        //    InitVar.Health = InitVar.Health + 2;
        //}
        //else if (canvas.tag == "ENDBOSS")
        //{
        //    Debug.Log("Box ENDBOSS+3");
        //    InitVar.Karma = InitVar.Karma + 3;
        //}





    }

    public void OnTriggerExit(Collider other)
    {
  


        charcter.SetActive(true);
        {
            ////Debug.Log("Box EXIT through!");
            ////Debug.Log("KARMA=" + InitVar.Karma + canvas.tag);
            ////Debug.Log("GOLD=" + InitVar.Gold + canvas.tag);
            //            Debug.Log("HEALTH=" + Health + canvas.tag);
            //            Debug.Log("IMMUNE=" + Immune + canvas.tag);
//            Debug.Log("KARMA_BACK=" + Karma_back + canvas.tag);
//            Debug.Log("GOLD=" + Gold_back + canvas.tag);
//            Debug.Log("HEALTH=" + Health_back + canvas.tag);
//            Debug.Log("IMMUNE=" + Immune_back + canvas.tag);
            //Debug.Log("///////////////////////////////////");

            canvas.SetActive(false);
            Screen.lockCursor = isLocked;
 //           Cursor.visible = false;
//                = Cursor.lockState = CursorLockMode.Locked;


        }


    }


    void Start()
    {

}


    void Update()
    {
        
    }
}
