
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
    public bool showingLogin = true;
    public GameObject TextLogin;
    public GameObject TextRegister;
    public GameObject InputAuthUsername;
    public GameObject TextAuthUsername;
    public GameObject InputAuthPassword;
    public GameObject TextAuthPassword;
    public GameObject InputConfirmPassword;
    public GameObject TextConfirmPassword;
    private String APIEndpoint = "http://192.168.0.13:4000/";

    public GameObject player;
    public bool inGameSignIn = false;

    private string LoggedInAsText = "Logged in as: ";
    private string NotLoggedInText = "Not logged in";

    // using this instead of start to prevent a null reference exception
    public void SetAuthRefs()
    {
        TextLogin = Manager.authCanvas.transform.Find("AuthenticationSystem/TextLogin").gameObject;
        TextRegister = Manager.authCanvas.transform.Find("AuthenticationSystem/TextRegister").gameObject;
        InputAuthUsername = Manager.authCanvas.transform.Find("AuthenticationSystem/InputAuthUsername").gameObject;
        TextAuthUsername = Manager.authCanvas.transform.Find("AuthenticationSystem/TextAuthUsername").gameObject;
        InputAuthPassword = Manager.authCanvas.transform.Find("AuthenticationSystem/InputAuthPassword").gameObject;
        TextAuthPassword = Manager.authCanvas.transform.Find("AuthenticationSystem/TextAuthPassword").gameObject;
        InputConfirmPassword = Manager.authCanvas.transform.Find("AuthenticationSystem/InputConfirmPassword").gameObject;
        TextConfirmPassword = Manager.authCanvas.transform.Find("AuthenticationSystem/TextConfirmPassword").gameObject;
    }

    public void ShowLogin()
    {
        Manager.authentication.showingLogin = true;
        Manager.authentication.TextLogin.SetActive(true);
        Manager.authentication.TextRegister.SetActive(false);
        Manager.authentication.InputConfirmPassword.SetActive(false);
        Manager.authentication.TextConfirmPassword.SetActive(false);
    }

    public void ShowRegister()
    {
        Manager.authentication.showingLogin = false;
        Manager.authentication.TextLogin.SetActive(false);
        Manager.authentication.TextRegister.SetActive(true);
        Manager.authentication.InputConfirmPassword.SetActive(true);
        Manager.authentication.TextConfirmPassword.SetActive(true);
    }

    public void GoBack()
    {
        if (Manager.authentication.inGameSignIn)
        {
            // this should go show the death canvas and hide the auth canvas
            Manager.authentication.inGameSignIn = false;
            Manager.authCanvas.SetActive(false);
            Manager.deathCanvas.SetActive(true);
        }
        else
        {
            Manager.authCanvas.SetActive(false);
            Manager.mainMenuCanvas.SetActive(true);
        }
    }

    public void Login()
    {
        if (!Manager.authentication.showingLogin)
        {
            Manager.authentication.ShowLogin();
        }
        else
        {
            string username = Manager.authentication.InputAuthUsername.GetComponent<TMP_InputField>().text;
            string password = Manager.authentication.InputAuthPassword.GetComponent<InputField>().text;
            Manager.authentication.StartCoroutine(PostLogin(username, password));
        }
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
                Debug.LogError(www.error);
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
                    Manager.userId = www.downloadHandler.text;
                    SetLogin(username);
                }
            }
        }
    }

    private void SetLogin(string username)
    {
        Debug.Log("Logging in to account!");
        Manager.loggedIn = true;
        Manager.loggedInUsername = username;
        Manager.mainMenuCanvas.transform.Find("LoggedInAs").gameObject.GetComponent<TMP_Text>().text = LoggedInAsText + Manager.loggedInUsername;

        // find a better way to loadscene
        if (Manager.authentication.inGameSignIn)
        {
            // submit hiscore, return to DeathCanvas somehow
            Manager.CollectStats();
            Manager.deathCanvas.SetActive(true);
            Manager.authCanvas.SetActive(false);
        }
        else
        {
            Manager.mainMenuCanvas.SetActive(true);
            Manager.authCanvas.SetActive(false);
        }
    }

    public void Register()
    {
        if (Manager.authentication.showingLogin)
        {
            Manager.authentication.ShowRegister();
        }
        else
        {
            string username = Manager.authentication.InputAuthUsername.GetComponent<TMP_InputField>().text;
            string password = Manager.authentication.InputAuthPassword.GetComponent<InputField>().text;
            string confirm = Manager.authentication.InputConfirmPassword.GetComponent<InputField>().text;
            if (password != confirm)
            {
                // throw an error, maybe change color or something.  Passwords must match before being sent to the server
                Debug.Log("Password doesn't match confirmed password");
            }
            else
            {
                Manager.authentication.StartCoroutine(PostRegister(username, password, confirm));
            }
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
        string username = Manager.authentication.InputAuthUsername.GetComponent<TMP_InputField>().text;
        Manager.authentication.StartCoroutine(PostOnline(username));
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

    public void SubmitScore(int shotsFired = 6, int shotsHit = 6, string accuracy = "666", int enemiesKilled = 6, int damageDone = 6, int roundsSurvived = 6)
    {
        // when this is called, the person should already be logged in
        if (Manager.loggedIn)
        {
            Manager.authentication.StartCoroutine(PostScore(shotsFired, shotsHit, accuracy, enemiesKilled, damageDone, roundsSurvived));
        }
    }

    IEnumerator PostScore(int shotsFired, int shotsHit, string accuracy, int enemiesKilled, int damageDone, int roundsSurvived)
    {
        // by the time this is called, the loggedInUsername will be set
        WWWForm form = new WWWForm();
        form.AddField("userId", Manager.userId);
        form.AddField("shotsFired", shotsFired);
        form.AddField("shotsHit", shotsHit);
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
                    Debug.Log("FALSE submitScore");
                }
                else if (www.downloadHandler.text == "scored")
                {
                    Debug.Log("Score submitted successfully");
                }
            }
        }
    }
}
