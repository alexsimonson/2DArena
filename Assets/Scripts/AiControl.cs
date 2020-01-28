using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiControl : MonoBehaviour
{

    private float walkSpeed = 5f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    public Weapon inHands;

    public BasicKnife knife;
    private GameObject weaponSlot;
    private Vector2 weaponSlotLocation;
    private bool isAttacking = false;

    private GameObject player;
    private Transform playerPos;

    public bool botShouldAttack = true;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        knife = new BasicKnife("Advanced Dagger", 100, .6f, "dagger 2");
        inHands = knife;
        weaponSlot = transform.GetChild(0).gameObject;
        weaponSlotLocation = weaponSlot.transform.localPosition;
        weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlot.GetComponent<SpriteRenderer> ().sortingLayerName = "Weapons";
    }

    void FixedUpdate()
    {
        Attack();
    }

    void Attack()
    {

        //should the bot choose to attack
        if (botShouldAttack)
        {
            //this will disable bot consecutive attacks
            StartCoroutine(botWaitToAttack());

            // Debug.Log("Bot Attack");
            isAttacking = true;

            if (inHands.type == 0)
            {
                // Debug.Log("Bot Attacking with a stab weapon: " + inHands.nameOf);
                StartCoroutine(Stab());

            }
            else if (inHands.type == 1)
            {
                // Debug.Log("Bot Attacking with a shoot weapon: " + inHands.nameOf);
            }
        }
    }

    private IEnumerator botWaitToAttack()
    {
        botShouldAttack = false;
        yield return new WaitForSeconds(3.0f);
        botShouldAttack = true;
    }


    private IEnumerator Stab()
    {
        Vector2 stabLocation = Vector2.up * 200.0f;
        Vector2 startStabLocation = weaponSlotLocation;
        weaponSlot.transform.localPosition = Vector3.Slerp(startStabLocation, stabLocation, Time.deltaTime);
        yield return new WaitForSeconds(0.2f);
        weaponSlot.transform.localPosition = weaponSlotLocation;
        isAttacking = false;
    }

}
