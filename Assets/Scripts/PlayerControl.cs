using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // initializing gameobject references
    public GameObject bullet;
    public GameObject player;
    // public GameObject playerLeftArm;
    // public GameObject playerRightArm;
    // public Fist fist;
    public Sprite playerForward;
    public Sprite playerBackward;
    public Weapon inHands;
    public Vector2 weaponSlotLocationLeft;
    public Vector2 weaponSlotLocationRight;
    public Vector3 diff;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private GameObject interactInRange = null;
    public GameObject weaponSlotLeft;
    public GameObject weaponSlotRight;

    // initializing local state variables
    public static bool hasControl;
    public bool player1 = true;
    public bool lookingLeft = false;
    private bool isAttacking = false;
    private float defaultWalkSpeed = 7f;
    private float backwardWalkSpeed = 4f;
    private float walkSpeed;
    private float sprintSpeed = 18f;

    // Use this for initialization
    void Start()
    {
        hasControl = true;
        walkSpeed = defaultWalkSpeed;
        player = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        weaponSlotLeft = GameObject.Find("WeaponSlotLeft");
        weaponSlotRight = GameObject.Find("WeaponSlotRight");
        UpdateWeaponSlotLocation();
        inHands = new Fist();
        weaponSlotLeft.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlotLeft.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";

        weaponSlotRight.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlotRight.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
        SwitchHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl)
        {
            MoveDirection();
            LookRotation();
            BodyRotation();
            SpeedManager();
            DodgeRoll();
        }
    }

    void SwitchHand()
    {
        weaponSlotLeft.SetActive(Manager.leftHanded);
        weaponSlotRight.SetActive(!Manager.leftHanded);
    }

    // you should inherently move slower when your back is turned
    void SpeedManager()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && this.diff.x < 0 && this.diff.y > 0)
        {
            walkSpeed = backwardWalkSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && this.diff.x < 0 && this.diff.y < 0)
        {
            walkSpeed = backwardWalkSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && this.diff.x > 0 && this.diff.y < 0)
        {
            walkSpeed = backwardWalkSpeed;
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && this.diff.x < 0 && this.diff.y > 0)
        {
            walkSpeed = defaultWalkSpeed;
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && this.diff.x > 0 && this.diff.y < 0)
        {
            walkSpeed = defaultWalkSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && this.diff.x < 0 || Input.GetAxisRaw("Horizontal") < 0 && this.diff.x > 0)
        {
            walkSpeed = backwardWalkSpeed;
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && this.diff.y < 0 || Input.GetAxisRaw("Vertical") < 0 && this.diff.y > 0)
        {
            walkSpeed = backwardWalkSpeed;
        }
        else
        {
            walkSpeed = defaultWalkSpeed;
        }
    }

    void BodyRotation()
    {
        //Subtracting the position of the player from the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();         //Normalizing the vector. Meaning that all the sum of the vector will be equal to 1

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;       //Find the angle in degrees
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);
    }

    void MoveDirection()
    {
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (player1)
        {
            // Debug.Log("Mouse: " + Input.mousePosition);
            // Debug.Log("Horizontal Axis: " + Input.GetAxisRaw("Horizontal"));
            // Debug.Log("Vertical Axis: " + Input.GetAxisRaw("Vertical"));
            // Debug.Log(this.diff);

            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rb.velocity = targetVelocity * walkSpeed;
        }
        else if (player1 == false)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("XboxHorizontal"), Input.GetAxisRaw("XboxVertical"));
            rb.velocity = targetVelocity * walkSpeed;
        }
        UpdateWeaponSlotLocation();
    }

    void UpdateWeaponSlotLocation()
    {
        weaponSlotLocationLeft = weaponSlotLeft.transform.position;
        weaponSlotLocationRight = weaponSlotRight.transform.position;
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

    private IEnumerator DodgeRoll()
    {
        if (Input.GetButtonDown("Roll") && hasControl)
        {
            hasControl = false;
            yield return new WaitForSeconds(2);
            hasControl = true;
        }
    }

    public void Dead()
    {
        hasControl = false;
        rb.velocity = Vector2.zero;
    }
}
