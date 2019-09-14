using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public Weapon inHands;
    public Fist fist;
    public GameObject weaponSlot;
    public Weapon[] weaponSlots;
    public GameObject bullet;

    private Vector2 weaponSlotLocation;
    private GameObject interactInRange = null;
    private int currentWeaponSlot = 0;
    private bool isAttacking = false;
    public bool hasControl, player1 = true;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        fist = new Fist();
        inHands = fist;
        weaponSlots = new Weapon[4];

        weaponSlot = transform.GetChild(0).gameObject;
        weaponSlotLocation = weaponSlot.transform.localPosition;
        weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlot.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";

    }

    // Update is called once per frame
    void Update()
    {
        SwapWeapon();
        Attack();
        ThrowWeapon();
    }

    void SwapWeapon()
    {
        if (Input.GetButtonDown("1"))
        {
            SetCurrentSlot(1);
        }
        if (Input.GetButtonDown("2"))
        {
            SetCurrentSlot(2);
        }
        if (Input.GetButtonDown("3"))
        {
            SetCurrentSlot(3);
        }
        if (Input.GetButtonDown("Interact"))
        {
            InteractWith(this.interactInRange);
        }

    }

    void PickupWeapon(GameObject colObj)
    {
        Weapon sw = colObj.GetComponent<PickupSpawner>().spawnedWeapon;
        // Debug.Log("Swapping " + inHands.nameOf + " for " + sw.nameOf);
        if (this.weaponSlots[1] == null)
        {
            this.currentWeaponSlot = 1;
            this.weaponSlots[this.currentWeaponSlot] = sw;
            this.inHands = sw;
        }
        else if (this.weaponSlots[2] == null)
        {
            this.currentWeaponSlot = 2;
        }

        this.weaponSlots[this.currentWeaponSlot] = sw;
        this.inHands = sw;
        this.weaponSlot.GetComponent<SpriteRenderer>().sprite = this.inHands.icon;
        Debug.Log("Current equipped weapon: " + this.inHands.nameOf + " - Slot " + this.currentWeaponSlot);
        Debug.Log("THE CURRENT WEAPON SLOTS: " + this.weaponSlots);
    }

    void changeSprite()
    {

    }

    //check which weapon is out and set accordingly
    void SetCurrentSlot(int x)
    {
        if (this.currentWeaponSlot == x)
        {
            // Pull out fists if you select the current weapon slot you're holding
            this.currentWeaponSlot = 0;
            this.weaponSlot.GetComponent<SpriteRenderer>().sprite = this.fist.icon;
            this.inHands = this.fist;
        }
        else
        {
            if (this.weaponSlots[x] != null)
            {
                //this should change weapon based on slot value x
                this.currentWeaponSlot = x;
                this.weaponSlot.GetComponent<SpriteRenderer>().sprite = this.weaponSlots[this.currentWeaponSlot].icon;
                this.inHands = this.weaponSlots[this.currentWeaponSlot];
            }
        }
    }

    void Attack()
    {
        if (player1)
        {
            if (Input.GetButtonDown("Fire1") && isAttacking != true)
            {
                isAttacking = true;
                if (this.inHands.type == 0)
                {
                    // Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
                    StartCoroutine(Stab());
                }
                else if (this.inHands.type == 1)
                {
                    // Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
                    StartCoroutine(Shoot(gameObject.transform.position));
                }
            }
        }
        else
        {

            if (Input.GetButtonDown("XboxFire1") && isAttacking != true)
            {
                Debug.Log("XboxFire1 is happening");
                isAttacking = true;
                if (this.inHands.type == 0)
                {
                    // Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
                    StartCoroutine(Stab());
                }
                else if (this.inHands.type == 1)
                {
                    // Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
                    StartCoroutine(Shoot(gameObject.transform.position));
                }
            }
        }
    }

    private IEnumerator Stab()
    {
        Vector2 stabLocation = Vector2.up * 200.0f;
        Vector2 startStabLocation = weaponSlotLocation;
        weaponSlot.transform.localPosition = Vector3.Slerp(startStabLocation, stabLocation, Time.deltaTime);
        weaponSlot.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(inHands.attackSpeed);
        weaponSlot.GetComponent<BoxCollider2D>().enabled = false;
        weaponSlot.transform.localPosition = weaponSlotLocation;
        isAttacking = false;
    }

    private IEnumerator Shoot(Vector2 gunStart)
    {
        if (player1)
        {
            Vector2 gunLocation = gunStart;
            Vector2 mouseLocation = Input.mousePosition;
            GameObject Newbullet = Instantiate(bullet, gunLocation, Quaternion.identity);
            Newbullet.GetComponent<bulletMovement>().targetForward = this.gameObject.transform.rotation * Vector2.up;
            Newbullet.GetComponent<bulletMovement>().bulletDamage = inHands.damage;
            yield return new WaitForSeconds(inHands.attackSpeed);
            isAttacking = false;
        }
        else
        {
            Vector2 gunLocation = gunStart;
            GameObject Newbullet = Instantiate(bullet, gunLocation, Quaternion.identity);
            Newbullet.GetComponent<bulletMovement>().targetForward = this.gameObject.transform.rotation * Vector2.up;
            Newbullet.GetComponent<bulletMovement>().bulletDamage = inHands.damage;
            yield return new WaitForSeconds(inHands.attackSpeed);
            isAttacking = false;
            // float rotationZ = this.gameObject.transform.rotation.z;
            // // rotationZ = Mathf.Atan2(-stickInput.x, stickInput.y) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0f, 0f, rotationZ -90);
        }

    }

    void ThrowWeapon()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (inHands.nameOf != "Fists")
            {
                this.weaponSlots[this.currentWeaponSlot] = null;
                this.currentWeaponSlot = 0;
                this.inHands = fist;
                this.weaponSlot.GetComponent<SpriteRenderer>().sprite = this.inHands.icon;
            }
        }
    }

    void InteractWith(GameObject colObj)
    {
        if (player1)
        {
            if (colObj != null)
            {
                if (colObj.tag == "Pickup")
                {
                    PickupWeapon(colObj);
                }
                else
                {
                    // Debug.Log("NO Pickup AVAILABLE");
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("XboxPickup"))
            {
                Debug.Log("I AM PICKUP");
                if (colObj != null)
                {
                    if (colObj.tag == "Pickup")
                    {
                        PickupWeapon(colObj);

                    }
                    else
                    {
                        // Debug.Log("NO Pickup AVAILABLE");
                    }
                }
                else
                {
                    // Debug.Log("Nothing in range");
                }
            }
        }
    }



    void OnTriggerExit2D(Collider2D col)
    {
        interactInRange = null;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        interactInRange = col.gameObject;
    }

}
