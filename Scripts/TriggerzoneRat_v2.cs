using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TriggerzoneRat_v2 : MonoBehaviour
{


    public GameObject panel;

    public GameObject canvas; //Your target for the refference

    public GameObject charcter;

   

    public void OnTriggerEnter(Collider other)
    {
        GameObject player = Player.player;
        panel.SetActive(player != null); // hide while not in the game world
        if (!player) return;



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
