using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMovement : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject[] players;
    GameObject player;
    public int bulletDamage;
    public float bulletSpeed = 100;
    public Vector2 targetForward;

    public bool firedByPlayer1 = true;
    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.Translate(targetForward * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Collider")
        {
            // Bullet hit wall
            Destroy(gameObject);
        }
        else if (col.tag == "Enemy" || col.tag == "EnemySpawner")
        {
            //this needs to apply damage from the weapon
            col.GetComponent<Health>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
