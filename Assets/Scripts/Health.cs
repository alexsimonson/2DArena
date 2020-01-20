﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // initializing local state variables
    public int maxHealth = 100;
    public int currentHealth = 100;
    private bool isDead = false;

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
            GameTypes.roundKillCount++;
            Destroy(gameObject);
        }
        if (gameObject.tag == "Player" && !this.isDead)
        {
            this.isDead = true;
            gameObject.GetComponent<PlayerControl>().Dead();
            Manager.DeathScreen();
        }
        //take away player input if player dies
    }
}
