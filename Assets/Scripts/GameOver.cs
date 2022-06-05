using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject fadeOut;
    public GameObject loadText;

    public void NewGameButton()
    {
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(NewGameStart());
    }

    IEnumerator NewGameStart()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(3);
        loadText.SetActive(true);
        SceneManager.LoadScene(0);
    }
}