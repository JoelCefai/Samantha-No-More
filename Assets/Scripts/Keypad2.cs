 using UnityEngine;
using System.Collections;

public class Keypad2 : MonoBehaviour
{

    public string curPassword = "12345";
    public string input;
    public bool onTrigger;
    public bool doorOpen;
    public bool keypadScreen;
    public Transform doorHinge;
    public GameObject bodka;
    public AudioSource Vrzganie;
    GUIStyle largeFont;
    GUIStyle InputFont;


    private void Start()
    {
        largeFont = new GUIStyle();
        largeFont.normal.textColor = Color.white;
        largeFont.fontSize = 50;
        InputFont = new GUIStyle();
        InputFont.normal.textColor = Color.white;
        InputFont.fontSize = 80;
    }


    void OnTriggerEnter(Collider other)
    {
        onTrigger = true;

    }

    void OnTriggerExit(Collider other)
    {
        onTrigger = false;
        keypadScreen = false;
        input = "";
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            input = input + "0";
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            input = input + "1";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            input = input + "2";
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            input = input + "3";
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            input = input + "4";
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            input = input + "5";
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            input = input + "6";
        }

        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            input = input + "7";
        }

        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            input = input + "8";
        }

        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            input = input + "9";
        }



        if (input == curPassword)
        {
            doorOpen = true;
        }

        if (doorOpen)
        {
            var newRot = Quaternion.RotateTowards(doorHinge.rotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), Time.deltaTime * 250);
            doorHinge.rotation = newRot;

            bodka.SetActive(false);
            Vrzganie.Play();


        }
    }

    void OnGUI()
    {
        if (!doorOpen)
        {
            if (onTrigger)
            {
                GUI.Box(new Rect(800, 500, 400, 50), "'E' to open keypad", largeFont);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    keypadScreen = true;
                    onTrigger = false;
                }
            }

            if (keypadScreen)
            {
                GUI.Box(new Rect(0, 0, 640, 70), "", largeFont);
                GUI.Box(new Rect(800, 10, 620, 50), input, InputFont);
                GUI.Label(new Rect(10, 60, 200, 50), "'Esc' to exit", largeFont);

                if (Input.GetKeyDown(KeyCode.Escape))
                {

                    keypadScreen = false;
                }


            }
        }
    }
}
