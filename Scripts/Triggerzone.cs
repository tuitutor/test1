using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerzone : MonoBehaviour
{
    public GameObject charcter;
    public void OnTriggerEnter(Collider other)
    {
        charcter.SetActive(false);
        {
            Debug.Log("Box went through!");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
