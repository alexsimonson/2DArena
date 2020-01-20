
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Authentication : MonoBehaviour
{
    public GameObject AuthCanvas;
    public GameObject TextLogin;
    public GameObject TextRegister;
    public GameObject InputAuthUsername;
    public GameObject TextAuthUsername;
    public GameObject InputAuthPassword;
    public GameObject TextAuthPassword;
    public GameObject InputConfirmPassword;
    public GameObject TextConfirmPassword;
    private String APIEndpoint = "http://192.168.0.13:3000/";

    public GameObject player;
    public bool inGameSignIn = false;

    void Start()
    {
        TextLogin = AuthCanvas.transform.FindChild("AuthenticationSystem/TextLogin").gameObject;
        TextRegister = AuthCanvas.transform.FindChild("AuthenticationSystem/TextRegister").gameObject;
        InputAuthUsername = AuthCanvas.transform.FindChild("AuthenticationSystem/InputAuthUsername").gameObject;
        TextAuthUsername = AuthCanvas.transform.FindChild("AuthenticationSystem/TextAuthUsername").gameObject;
        InputAuthPassword = AuthCanvas.transform.FindChild("AuthenticationSystem/InputAuthPassword").gameObject;
        TextAuthPassword = AuthCanvas.transform.FindChild("AuthenticationSystem/TextAuthPassword").gameObject;
        InputConfirmPassword = AuthCanvas.transform.FindChild("AuthenticationSystem/InputConfirmPassword").gameObject;
        TextConfirmPassword = AuthCanvas.transform.FindChild("AuthenticationSystem/TextConfirmPassword").gameObject;
    }

    public void Login()
    {
        Debug.Log("PRESSING LOGIN");
        string username = GameObject.Find("InputAuthUsername").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputAuthPassword").GetComponent<InputField>().text;
        StartCoroutine(PostLogin(username, password));
    }

    IEnumerator PostLogin(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(APIEndpoint + "login", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("RESPONSE CODE: " + www.downloadHandler.text);
                if (www.downloadHandler.text == "false")
                {
                    Debug.LogError("No account exists with that username");
                }
                else if (www.downloadHandler.text == "nomatch")
                {
                    Debug.LogError("Passwords didn't match, please try again.");
                }
                else if (www.downloadHandler.text == "wrong")
                {
                    Debug.LogError("Something went wrong... likely a firebase issue");
                }
                else
                {
                    SetLogin(username);
                }
            }
        }
    }

    private void SetLogin(string username)
    {
        Debug.Log("Logging in to account!");
        gameObject.GetComponent<GameAssistantToTheManager>().isLoggedIn = true;
        gameObject.GetComponent<GameAssistantToTheManager>().loggedInUsername = username;
        // find a better way to loadscene
        if (inGameSignIn)
        {
            // submit hiscore, return to DeathCanvas somehow
            SubmitScore();
            this.gameObject.GetComponent<GameAssistantToTheManager>().deathRef.SetActive(true);
            this.gameObject.GetComponent<GameAssistantToTheManager>().authRef.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Register()
    {
        string username = InputAuthUsername.GetComponent<TMP_InputField>().text;
        string password = InputAuthPassword.GetComponent<InputField>().text;
        string confirm = InputConfirmPassword.GetComponent<InputField>().text;
        if (password != confirm)
        {
            // throw an error, maybe change color or something.  Passwords must match before being sent to the server
            Debug.Log("Password doesn't match confirmed password");
        }
        else
        {
            StartCoroutine(PostRegister(username, password, confirm));
        }
    }

    IEnumerator PostRegister(string username, string password, string confirm)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("confirm", confirm);

        using (UnityWebRequest www = UnityWebRequest.Post(APIEndpoint + "register", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("RESPONSE CODE: " + www.downloadHandler.text);
                if (www.downloadHandler.text == "false")
                {
                    Debug.Log("Likely a problem with firebase");
                }
                else if (www.downloadHandler.text == "exist")
                {
                    Debug.Log("Account already registered with that username");
                }
                else if (www.downloadHandler.text == "true")
                {
                    Debug.Log("New account registered!");
                }
            }
        }
    }

    public void Online()
    {
        string username = InputAuthUsername.GetComponent<TMP_InputField>().text;
        StartCoroutine(PostOnline(username));
    }

    IEnumerator PostOnline(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        using (UnityWebRequest www = UnityWebRequest.Post(APIEndpoint + "online", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("RESPONSE CODE: " + www.downloadHandler.text);
                if (www.downloadHandler.text == "false")
                {
                    Debug.LogError("FALSE ONLINE");
                }
                else if (www.downloadHandler.text == "true")
                {
                    Debug.Log("TRUE ONLINE");
                }
            }
        }
    }

    public void SubmitScore(int shotsFired = 6, double accuracy = 6, int enemiesKilled = 6, int damageDone = 6, int roundsSurvived = 6)
    {
        // when this is called, the person should already be logged in
        if (gameObject.GetComponent<GameAssistantToTheManager>().isLoggedIn)
        {
            StartCoroutine(PostScore(shotsFired, accuracy, enemiesKilled, damageDone, roundsSurvived));
        }
    }

    IEnumerator PostScore(int shotsFired, double accuracy, int enemiesKilled, int damageDone, int roundsSurvived)
    {
        // by the time this is called, the loggedInUsername will be set
        WWWForm form = new WWWForm();
        form.AddField("username", gameObject.GetComponent<GameAssistantToTheManager>().loggedInUsername);
        form.AddField("shotsFired", shotsFired);
        form.AddField("accuracy", accuracy.ToString());
        form.AddField("enemiesKilled", enemiesKilled);
        form.AddField("damageDone", damageDone);
        form.AddField("roundsSurvived", roundsSurvived);

        using (UnityWebRequest www = UnityWebRequest.Post(APIEndpoint + "submitScore", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("RESPONSE CODE: " + www.downloadHandler.text);
                if (www.downloadHandler.text == "false")
                {
                    Debug.LogError("FALSE submitScore");
                }
                else if (www.downloadHandler.text == "true")
                {
                    Debug.Log("TRUE submitScore");
                }
            }
        }
    }
}
