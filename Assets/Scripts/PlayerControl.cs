using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // initializing gameobject references
    public GameObject bullet;
    public GameObject player;
    public GameObject playerLeftArm;
    public GameObject playerRightArm;
    public Fist fist;
    public Sprite playerForward;
    public Sprite playerBackward;
    public Weapon inHands;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private GameObject interactInRange = null;
    private GameObject weaponSlotLeft;
    private GameObject weaponSlotRight;
    private Vector2 weaponSlotLocationLeft;
    private Vector2 weaponSlotLocationRight;

    // initializing local state variables
    public bool hasControl = true;
    public bool player1 = true;
    private bool isAttacking = false;
    private float walkSpeed = 15f;

    // Use this for initialization
    void Start()
    {
        player = this.gameObject;
        playerLeftArm = GameObject.Find("LeftArmShoulderPivot");
        playerRightArm = GameObject.Find("RightArmShoulderPivot");
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        fist = new Fist();
        inHands = fist;

        //left hand
        weaponSlotLeft = GameObject.Find("WeaponSlotLeft");
        weaponSlotLocationLeft = weaponSlotLeft.transform.localPosition;
        weaponSlotLeft.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlotLeft.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";

        //right hand
        weaponSlotRight = GameObject.Find("WeaponSlotRight");
        weaponSlotLocationRight = weaponSlotRight.transform.localPosition;
        weaponSlotRight.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlotRight.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl)
        {
            MoveDirection();
            LookRotation();
            SpriteManager();
        }
    }

    void SpriteManager()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        // Debug.Log("DIFF X: " + diff.x);
        if (diff.x > 0)
        {
            // this would be on the right side of the screen
            // Debug.Log("Mouse on the right half of screen");
            this.player.GetComponent<SpriteRenderer>().flipX = false;
            // this is for disabling the left arm and enabling the right arm
            this.playerLeftArm.SetActive(false);
            this.playerRightArm.SetActive(true);
        }
        else
        {
            // this would be on the left side of the screen
            // Debug.Log("Mouse on the left half of screen");
            this.player.GetComponent<SpriteRenderer>().flipX = true;

            // this is for disabling the right arm and enabling the left arm
            this.playerLeftArm.SetActive(true);
            this.playerRightArm.SetActive(false);

            // this is because guns are upside down if you don't flip this melee shouldn't be effected
            weaponSlotLeft.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (rotationZ > 0)
        {
            // this would be on top of the screen
            // Debug.Log("Mouse on the top of the screen");
            if (this.player.GetComponent<SpriteRenderer>().sprite.name == "DemonHunter-Default")
            {
                this.player.GetComponent<SpriteRenderer>().sprite = playerBackward;
            }
            // this is for flipping the arm when going top to bottom
            this.playerLeftArm.GetComponent<SpriteRenderer>().flipX = true;
            this.playerRightArm.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            // this would be on the bottom of the screen
            // Debug.Log("Mouse on the bottom of the screen");
            if (this.player.GetComponent<SpriteRenderer>().sprite.name == "DemonHunter-Default Up")
            {
                this.player.GetComponent<SpriteRenderer>().sprite = playerForward;
            }
            // this is for flipping the arm when going top to bottom
            this.playerLeftArm.GetComponent<SpriteRenderer>().flipX = false;
            this.playerRightArm.GetComponent<SpriteRenderer>().flipX = false;
        }
        // diff x is left and right
        // rotation z is up and down
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
