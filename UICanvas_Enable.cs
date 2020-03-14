using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas_Enable : MonoBehaviour
{
    public GameObject canvas; //Your target for the refference

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        canvas.SetActive(true);
    }
}
