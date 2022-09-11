using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public string name;
    GUIStyle largeFont;




    void Start()
    {
        largeFont = new GUIStyle();
        largeFont.normal.textColor = Color.white;
        largeFont.fontSize = 50;
        camera1.enabled = true;
        camera2.enabled = false;
        name = "Jeremy";



    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            camera1.enabled = !camera1.enabled;            
            camera2.enabled = !camera2.enabled;

          


        }

    }

}

