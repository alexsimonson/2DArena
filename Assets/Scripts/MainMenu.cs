using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject Music;
    public GameObject GameManager;
    public GameObject LoggedInAs;
    private string loginStatus = "Logged in as ";

    void Start()
    {
        Music = GameObject.Find("Music");
        Music.GetComponent<AudioSource>();
        GameManager = GameObject.FindWithTag("GameManager");
        LoggedInAs = GameObject.Find("MainMenuCanvas/LoggedInAs");

        if (GameManager.GetComponent<GameAssistantToTheManager>().isLoggedIn)
        {
            LoggedInAs.SetActive(true);
            LoggedInAs.GetComponent<TMP_Text>().text = loginStatus + GameManager.GetComponent<GameAssistantToTheManager>().loggedInUsername;
        }
        else
        {
            LoggedInAs.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        Debug.Log("NO OPTIONS MATE");
    }

    public void Login()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
