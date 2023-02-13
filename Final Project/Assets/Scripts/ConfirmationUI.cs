using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConfirmationUI : MonoBehaviour
{
    public GameObject ConfirmUI;

    public void ExitGameFromPause()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenConfirmation()
    {
        ConfirmUI.SetActive(true);
    }

    public void DontWantToExitGame()
    {
        ConfirmUI.SetActive(false);
    }

}
