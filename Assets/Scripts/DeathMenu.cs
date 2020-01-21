using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public void RetryGame()
    {
        Manager.deathCanvas.SetActive(false);
        GameTypes.StartGameMode();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SubmitScore()
    {
        // this requires a login
        if (!Manager.loggedIn)
        {
            Manager.authentication.inGameSignIn = true;
            Manager.authCanvas.SetActive(true);
            Manager.deathCanvas.SetActive(false);
        }
    }
}
