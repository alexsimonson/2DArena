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
    public static Health playerHealth;
    public static PlayerControl playerControl;
    public static WeaponSystem weaponSystem;
    public static AudioSource musicAudioSource;

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
        weaponSystem = player.GetComponent<WeaponSystem>();
        playerHealth = player.GetComponent<Health>();
        playerUI = gameObject.GetComponent<PlayerUI>();
        healthUI = gameObject.GetComponent<HealthUI>();
        inventoryUI = gameObject.GetComponent<InventoryUI>();
        authentication = gameObject.GetComponent<Authentication>();
        mainMenu = gameObject.GetComponent<MainMenu>();
        playerControl = player.GetComponent<PlayerControl>();
        musicAudioSource = gameObject.GetComponent<AudioSource>();
        authentication.SetAuthRefs();
        player.SetActive(false);
        hudCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        authCanvas.SetActive(false);
        musicAudioSource.Play();
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
            Manager.accuracy = "null";
        }
        else
        {
            var temp = (double)shotsHit / shotsFired;
            Manager.accuracy = ((double)temp * 100).ToString();
        }
        Manager.authentication.SubmitScore(Manager.shotsFired, Manager.shotsHit, Manager.accuracy, Manager.enemiesKilled, Manager.damageDone, Manager.roundsSurvived);
    }

    public static void ResetStats()
    {
        shotsFired = 0;
        shotsHit = 0;
        enemiesKilled = 0;
        damageDone = 0;
        roundsSurvived = 0;
    }

    public static void ResetPlayerHealth()
    {
        Manager.playerHealth.currentHealth = 100;
        Manager.healthUI.SetHealthCount(100);
        Manager.playerHealth.isDead = false;
    }

    public static void ResetGame()
    {
        Manager.ResetStats();
        Manager.ResetPlayerHealth();
        Manager.inventoryUI.HideInventory();
        Manager.weaponSystem.weaponSlots = new Weapon[4];
        PlayerControl.hasControl = true;
        Manager.player.transform.position = LevelSetup.playerSpawn.transform.position;
        Manager.weaponSystem.inHands = Manager.weaponSystem.fist;
        // Manager.weaponSystem.SetWeaponSlotSprites();
    }
}
