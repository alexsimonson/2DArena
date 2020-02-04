using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    // initializing gameobject references
    public GameObject bullet;
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
    public static bool isTrespassing;
    public bool lookingLeft = false;
    private bool isAttacking = false;
    public bool sprinting = false;
    private float defaultWalkSpeed = 7f;
    private float backwardWalkSpeed = 4f;
    private float walkSpeed;
    private float sprintSpeed = 18f;

    private Vector2 targetVelocity;

    // Use this for initialization
    void Start () {
        hasControl = true;
        isTrespassing = false;
        walkSpeed = defaultWalkSpeed;
        rb = GetComponent<Rigidbody2D> ();
        bc = GetComponent<BoxCollider2D> ();
        diff = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;

        weaponSlotLeft = GameObject.Find ("WeaponSlotLeft");
        weaponSlotRight = GameObject.Find ("WeaponSlotRight");
        UpdateWeaponSlotLocation ();
        inHands = new Fist ();
        weaponSlotLeft.GetComponent<SpriteRenderer> ().sprite = inHands.icon;
        weaponSlotLeft.GetComponent<SpriteRenderer> ().sortingLayerName = "Weapons";

        weaponSlotRight.GetComponent<SpriteRenderer> ().sprite = inHands.icon;
        weaponSlotRight.GetComponent<SpriteRenderer> ().sortingLayerName = "Weapons";
        SwitchHand ();
    }

    // Update is called once per frame
    void Update () {
        if (hasControl) {
            MoveDirection ();
            LookRotation ();
            SpeedManager ();
            DodgeRoll ();
            if(Input.GetButtonDown("SwitchHand")){
                SwitchHand();
            }
        }
    }

    void SwitchHand () {
        Manager.leftHanded = !Manager.leftHanded;
        weaponSlotLeft.SetActive (Manager.leftHanded);
        weaponSlotRight.SetActive (!Manager.leftHanded);
    }

    // you should inherently move slower when your back is turned
    void SpeedManager () {
        if(Input.GetAxis("Sprint") > 0){
            sprinting = true;
            // increased movement, regardless of mouse direction (turn character to face that way)
            walkSpeed = sprintSpeed;
        }else{
            sprinting = false;
            // normal movement and walking speed when sprint isn't pressed
            if (Input.GetAxisRaw ("Horizontal") > 0 && this.diff.x < 0 && this.diff.y > 0) {
                walkSpeed = backwardWalkSpeed;
            } else if (Input.GetAxisRaw ("Horizontal") > 0 && this.diff.x < 0 && this.diff.y < 0) {
                walkSpeed = backwardWalkSpeed;
            } else if (Input.GetAxisRaw ("Horizontal") < 0 && this.diff.x > 0 && this.diff.y < 0) {
                walkSpeed = backwardWalkSpeed;
            } else if (Input.GetAxisRaw ("Vertical") > 0 && this.diff.x < 0 && this.diff.y > 0) {
                walkSpeed = defaultWalkSpeed;
            } else if (Input.GetAxisRaw ("Vertical") < 0 && this.diff.x > 0 && this.diff.y < 0) {
                walkSpeed = defaultWalkSpeed;
            } else if (Input.GetAxisRaw ("Horizontal") > 0 && this.diff.x < 0 || Input.GetAxisRaw ("Horizontal") < 0 && this.diff.x > 0) {
                walkSpeed = backwardWalkSpeed;
            } else if (Input.GetAxisRaw ("Vertical") > 0 && this.diff.y < 0 || Input.GetAxisRaw ("Vertical") < 0 && this.diff.y > 0) {
                walkSpeed = backwardWalkSpeed;
            } else {
                walkSpeed = defaultWalkSpeed;
            }
        }
    }

    void LookRotation () {
        if(!sprinting){
            //Subtracting the position of the player from the mouse position
            Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
            difference.Normalize (); //Normalizing the vector. Meaning that all the sum of the vector will be equal to 1

            float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg; //Find the angle in degrees
            gameObject.transform.rotation = Quaternion.Euler (0f, 0f, rotZ + 90);
        }else{
            // rotate in whatever direction you are moving
            float rotZ = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

        }
    }

    void MoveDirectionController () {
        Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        rb.velocity = targetVelocity * walkSpeed;
        UpdateWeaponSlotLocation ();
    }

    void MoveDirection () {
        diff = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
        targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        rb.velocity = targetVelocity * walkSpeed;
        UpdateWeaponSlotLocation ();
    }

    void UpdateWeaponSlotLocation () {
        weaponSlotLocationLeft = weaponSlotLeft.transform.position;
        weaponSlotLocationRight = weaponSlotRight.transform.position;
    }

    private IEnumerator DodgeRoll () {
        if (Input.GetButtonDown ("Roll") && hasControl) {
            hasControl = false;
            yield return new WaitForSeconds (2);
            hasControl = true;
        }
    }

    public void Dead () {
        hasControl = false;
        rb.velocity = Vector2.zero;
    }
}