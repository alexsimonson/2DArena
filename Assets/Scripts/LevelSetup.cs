using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public static GameObject playerSpawn;
    // Start is called before the first frame update
    void Start()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        GameTypes.StartGameMode();
    }
}
