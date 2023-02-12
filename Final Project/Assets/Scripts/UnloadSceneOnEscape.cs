using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadSceneOnEscape : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync("Help");
        }
    }

    public void ExitHelp()
    {
        SceneManager.UnloadSceneAsync("Help");
    }
}
