using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    private BoxCollider2D bc;
    private GameObject self;
    // Use this for initialization
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        self = gameObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (self.tag == "PlayerWeapon")
        {
            if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemySpawner")
            {
                col.gameObject.GetComponent<Health>().TakeDamage(self.GetComponentInParent<WeaponSystem>().inHands.damage);
            }
            if (col.gameObject.tag == "EnemyWeapon")
            {
                // could maybe knock weapons out of enemies hands
                Debug.Log("Player hit enemy weapon");
            }
        }
        else if (self.tag == "EnemyWeapon")
        {
            if (col.gameObject.tag == "PlayerHitbox")
            {
                col.gameObject.GetComponentInParent<Health>().TakeDamage(self.GetComponentInParent<AiControl>().inHands.damage, true);
            }
        }
    }
}
