using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static bool leftHanded;
    public static bool loggedIn;
    public static int gameMode;
    public static int spawnersLeft;
    public static int enemiesLeft = 0;
    public static GameObject player;
    public static GameObject deathScreen;
    public static GameObject mainCam;

    public static GameObject hudCanvas;

    public static PlayerUI playerUI;
    public static HealthUI healthUI;
    public static InventoryUI inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        hudCanvas = GameObject.Find("hudCanvas");
        DontDestroyOnLoad(mainCam);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(hudCanvas);
        player.SetActive(false);
        hudCanvas.SetActive(false);
        playerUI = gameObject.GetComponent<PlayerUI>();
        healthUI = gameObject.GetComponent<HealthUI>();
        inventoryUI = gameObject.GetComponent<InventoryUI>();
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        // gameObject.GetComponent<GameTypes>().UpdateGameMode();
    }

    void Awake()
    {
        Debug.Log("Awaken my child");
        // SceneManager.sceneLoaded += OnSceneLoaded;
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
        Instantiate(deathScreen, mainCam.transform.position, Quaternion.identity);    //this pops ui but isn't usable
    }
}
