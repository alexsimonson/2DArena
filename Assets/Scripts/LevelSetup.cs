using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting");
        // Manager.gameMode = 1;
        GameTypes.StartGameMode();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
