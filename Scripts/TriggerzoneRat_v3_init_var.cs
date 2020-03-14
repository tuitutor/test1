using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TriggerzoneRat_v3_init_var : MonoBehaviour
{


    //    public int baseRecoveryPerTick = 0;
    //   public LevelBasedInt baseHealth = new LevelBasedInt { baseValue = 100 };




    public GameObject canvas; //Your target for the refference

    public GameObject charcter;



    public void OnTriggerEnter(Collider other)
    {



        charcter.SetActive(false);
        {
            Debug.Log("Box went through!");
            canvas.SetActive(true);
            
            




        }

    }

    public void OnTriggerExit(Collider other)
    {
  


        charcter.SetActive(true);
        {
            Debug.Log("Box EXIT through!");
            Debug.Log("///////////////////////////////////");

            canvas.SetActive(false);


        }


    }


    void Start()
    {

}


    void Update()
    {
        
    }
}
