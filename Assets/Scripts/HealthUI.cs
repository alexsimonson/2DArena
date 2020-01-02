
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public GameObject player;
    public GameObject hudCanvas;
    public GameObject healthCount;
    public GameObject healthTitle;

    void Start()
    {
        player = GameObject.Find("PlayerSprite");
        hudCanvas = GameObject.Find("hudCanvas");
        healthCount = GameObject.Find("hudCanvas/hudHealth/healthCount");
        healthTitle = GameObject.Find("hudCanvas/hudHealth/healthTitle");
    }

    public void SetHealthCount(int health)
    {
        healthCount.GetComponent<TMP_Text>().text = health.ToString();
    }
}
