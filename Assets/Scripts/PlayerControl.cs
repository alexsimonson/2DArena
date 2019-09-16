using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private float walkSpeed = 15f;

    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public Weapon inHands;
    public Fist fist;

    private GameObject interactInRange = null;

    private GameObject weaponSlot;
    private Vector2 weaponSlotLocation;

    private bool isAttacking = false;

    public GameObject bullet;

    public bool hasControl = true;
    public bool player1 = true;

    public Sprite playerForward;
    public Sprite playerBackward;
    public GameObject player;
    // Use this for initialization
    void Start()
    {
        player = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        fist = new Fist();
        inHands = fist;
        weaponSlot = transform.GetChild(0).gameObject;
        weaponSlotLocation = weaponSlot.transform.localPosition;
        weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlot.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl)
        {
            MoveDirection();
            LookRotation();
            spriteManager();
        }
    }

    void spriteManager(){
        // Debug.Log("THIS IS THE FLIP : " + this.player.GetComponent<SpriteRenderer>().flip.y);
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        // Debug.Log("DIFF X: " + diff.x);
        if(diff.x > 0){
            // this would be on the right side of the screen
            Debug.Log("Mouse on the right half of screen");
            this.player.GetComponent<SpriteRenderer>().flipX = false;            
        }else{
            // this would be on the left side of the screen
            Debug.Log("Mouse on the left half of screen");
            this.player.GetComponent<SpriteRenderer>().flipX = true;
        }

        if(rotationZ > 0){
            // this would be on top of the screen
            Debug.Log("Mouse on the top of the screen");
            if(this.player.GetComponent<SpriteRenderer>().sprite.name=="DemonHunter-Default"){
                this.player.GetComponent<SpriteRenderer>().sprite = playerBackward;   
            }
        }else{
            // this would be on the bottom of the screen
            Debug.Log("Mouse on the bottom of the screen");
            if(this.player.GetComponent<SpriteRenderer>().sprite.name=="DemonHunter-Default Up"){
                this.player.GetComponent<SpriteRenderer>().sprite = playerForward;   
            }
        }
        // diff x is left and right
        // rotation z is up and down
        // transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
        Debug.Log("ROTATION Z: " + rotationZ);
    }

    void MoveDirection()
    {
        if (player1)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rb.velocity = targetVelocity * walkSpeed;
        }
        else if (player1 == false)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("XboxHorizontal"), Input.GetAxisRaw("XboxVertical"));
            rb.velocity = targetVelocity * walkSpeed;
        }
    }

    void LookRotation()
    {
        if (player1)
        {
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            diff.Normalize();
            float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
        }
        else if (player1 == false)
        {
            float inputHoriz = Input.GetAxisRaw("XboxStickHorizontal");
            float inputVert = Input.GetAxisRaw("XboxStickVertical");
            float deadzone = 0.25f;
            Vector2 stickInput = new Vector2(Input.GetAxisRaw("XboxStickVertical"), Input.GetAxisRaw("XboxStickHorizontal"));
            if (stickInput.magnitude < deadzone)
                stickInput = Vector2.zero;
            else
            {
                float rotationZ = Mathf.Atan2(-stickInput.x, stickInput.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
            }
        }
    }
}
