using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject[] players;
    GameObject player;
    public int bulletDamage;

    // Use this for initialization

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Collider")
        {
            // Bullet hit wall
            Destroy(gameObject);
        }
        else if (col.tag == "Enemy" || col.tag == "EnemySpawner")
        {
            Manager.shotsHit++;
            //this needs to apply damage from the weapon
            col.GetComponent<Health>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
