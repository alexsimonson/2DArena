using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static bool leftHanded;
    public static bool loggedIn;
    public static string loggedInUsername;
    public static string userId;
    public static int gameMode;
    public static int spawnersLeft;
    public static int enemiesLeft = 0;
    public static GameObject player;
    public static GameObject mainCam;

    public static GameObject hudCanvas;
    public static GameObject deathCanvas;
    public static GameObject authCanvas;
    public static GameObject mainMenuCanvas;

    public static PlayerUI playerUI;
    public static HealthUI healthUI;
    public static InventoryUI inventoryUI;
    public static MainMenu mainMenu;
    public static Authentication authentication;

    public static int shotsFired;
    public static int shotsHit;
    public static string accuracy;
    public static int enemiesKilled;
    public static int damageDone;
    public static int roundsSurvived;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        hudCanvas = GameObject.Find("hudCanvas");
        deathCanvas = GameObject.Find("DeathCanvas");
        authCanvas = GameObject.Find("AuthCanvas");
        mainMenuCanvas = GameObject.Find("MainMenuCanvas");

        DontDestroyOnLoad(mainCam);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(hudCanvas);
        DontDestroyOnLoad(deathCanvas);
        DontDestroyOnLoad(authCanvas);
        DontDestroyOnLoad(mainMenuCanvas);
        player.SetActive(false);
        hudCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        authCanvas.SetActive(false);
        playerUI = gameObject.GetComponent<PlayerUI>();
        healthUI = gameObject.GetComponent<HealthUI>();
        inventoryUI = gameObject.GetComponent<InventoryUI>();
        authentication = gameObject.GetComponent<Authentication>();
        mainMenu = gameObject.GetComponent<MainMenu>();
        authentication.SetAuthRefs();
        SceneManager.LoadScene(1);
    }

    void Awake()
    {
        Debug.Log("Awaken my child");
    }

    void OnSceneLoaded()
    {
        Manager.gameMode = 1;   // temporarily setting the game mode here
    }

    //based on tag, add or remove to the count tracked by scoreboard
    public static void ScoreUpdate(string tagName, bool increase)
    {
        if (tagName == "Enemy")
        {
            if (increase)
            {
                enemiesLeft++;
                //				Debug.Log("Enemy # increased to " + enemiesLeft);
            }
            else
            {
                enemiesLeft--;
                //				Debug.Log("Enemy # DECREASED to " + enemiesLeft);
            }
        }
        else if (tagName == "EnemySpawner")
        {
            if (increase)
            {
                spawnersLeft++;
                //				Debug.Log("Enemy spawners # increased to " + spawnersLeft);
            }
            else
            {
                spawnersLeft--;
                //				Debug.Log("Enemy spawners # DECREASED to " + spawnersLeft);
            }
        }
    }

    public static void DeathScreen()
    {
        deathCanvas.SetActive(true);
    }

    public static void CollectStats()
    {
        // calculate accuracy
        if (shotsFired <= 0)
        {
            accuracy = "null";
        }
        else
        {
            var temp = (double)shotsHit / shotsFired;
            accuracy = (double)temp * 100 + "%";
        }
        Manager.authentication.SubmitScore(shotsFired, shotsHit, accuracy, enemiesKilled, damageDone, roundsSurvived);
    }
}
