using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public GameObject Auth;
    public GameObject player;
    public GameObject submitButton;
    public GameObject GameManager;
    // Use this for initialization
    void Start()
    {
        Auth = GameObject.FindWithTag("Auth");
        player = GameObject.FindWithTag("Player");
        submitButton = GameObject.Find("DeathCanvas/MiddlePanel/ButtonPanel/SubmitButton");
        GameManager = GameObject.FindWithTag("GameManager");
        if (Auth == null)
        {
            // not logged in
        }
        else
        {
            submitButton.SetActive(false);
            // should be logged in and hiscore should be submitted automatically
            player.GetComponent<Score>().CollectStats();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SubmitScore()
    {
        // this requires a login
        GameManager.GetComponent<GameAssistantToTheManager>().AuthScreen();
        // open the authcanvas
        // SceneManager.LoadScene(2);
    }
}
