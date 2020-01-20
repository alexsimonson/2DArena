using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        Manager.gameMode = 1;
        SceneManager.LoadScene(2);
    }

    public void Options()
    {
        if (Manager.leftHanded)
        {
            Manager.leftHanded = false;
        }
        else
        {
            Manager.leftHanded = true;
        }
        Debug.Log("leftHanded set to " + Manager.leftHanded);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
