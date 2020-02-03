using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        Manager.gameMode = 0;
        Manager.mainMenuCanvas.SetActive(false);
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

    public void Login()
    {
        Manager.authCanvas.SetActive(true);
        Manager.mainMenuCanvas.SetActive(false);
        Manager.authentication.ShowLogin();
    }

    public void MuteMusic()
    {
        Manager.musicAudioSource.mute = !Manager.musicAudioSource.mute;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
