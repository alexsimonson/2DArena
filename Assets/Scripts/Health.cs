using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    // initializing gameobject references
    public GameObject GameManager;

    // initializing local state variables
    public int maxHealth = 100;
    public int currentHealth = 100;

    // Use this for initialization
    void Start()
    {
        GameManager = GameObject.FindWithTag("GameManager");
    }

    public void TakeDamage(int damage, bool player = false)
    {
        currentHealth -= damage;
        if (player)
        {
            gameObject.GetComponent<HealthUI>().SetHealthCount(currentHealth);
        }
        CheckDead();
    }

    void CheckDead()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        if (gameObject.tag == "Enemy")
        {
            GameManager.GetComponent<GameTypes>().roundKillCount++;
            Destroy(gameObject);
        }
        if (gameObject.tag == "Player")
        {
            gameObject.GetComponent<PlayerControl>().hasControl = false;
            GameManager.GetComponent<GameAssistantToTheManager>().DeathScreen();
        }
        //take away player input if player dies
    }
}
