using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // initializing local state variables
    public int maxHealth = 100;
    public int currentHealth = 100;
    public bool isDead = false;

    public void TakeDamage(int damage, bool player = false)
    {
        currentHealth -= damage;
        if (player)
        {
            Manager.healthUI.SetHealthCount(currentHealth);
        }
        else
        {
            Manager.damageDone += damage;
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
            Manager.enemiesKilled++;
            GameTypes.roundKillCount++;
            Destroy(gameObject);
        }
        if (gameObject.tag == "Player" && !this.isDead)
        {
            this.isDead = true;
            PlayerControl.hasControl = false;
            GameTypes.gameStarted = false;
            gameObject.GetComponent<PlayerControl>().Dead();
            if (Manager.loggedIn)
            {
                Manager.CollectStats();
            }
            Manager.DeathScreen();
        }
        //take away player input if player dies
    }
}
