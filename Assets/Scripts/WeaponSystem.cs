using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // initializing gameobject references
    public GameObject player;
    public GameObject bullet;
    public GameObject weaponSlotLeft;
    public GameObject weaponSlotRight;
    public Fist fist;
    public Weapon inHands;
    public Weapon[] weaponSlots;

    private GameObject interactInRange = null;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private Vector2 weaponSlotLocationLeft;
    private Vector2 weaponSlotLocationRight;

    // initializing local state variables
    public bool hasControl, player1 = true;

    private int currentWeaponSlot = 0;
    private bool isAttacking = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("PlayerSprite");
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        fist = new Fist();
        inHands = fist;
        weaponSlots = new Weapon[4];

        weaponSlotLeft = GameObject.Find("WeaponSlotLeft");
        weaponSlotRight = GameObject.Find("WeaponSlotRight");

        weaponSlotLeft.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
        weaponSlotRight.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";

        weaponSlotLocationLeft = weaponSlotLeft.transform.localPosition;
        weaponSlotLocationRight = weaponSlotRight.transform.localPosition;

        SetWeaponSlotSprites();
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        Attack();
        ThrowWeapon();
    }

    void DetectInput()
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
        if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }

    }

    void PickupWeapon(GameObject colObj)
    {
        Weapon sw = colObj.GetComponent<PickupSpawner>().spawnedWeapon;
        Debug.Log(weaponSlots);
        foreach (Weapon x in weaponSlots)
        {
            if (x != null && x.nameOf == sw.nameOf)
            {
                // just add ammo
                x.ammoPool += sw.addAmmo;
                return;
            }
        }
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
        SetWeaponSlotSprites();
        Debug.Log("Current equipped weapon: " + this.inHands.nameOf + " - Slot " + this.currentWeaponSlot);
        Debug.Log("THE CURRENT WEAPON SLOTS: " + this.weaponSlots);
    }

    void SetWeaponSlotSprites()
    {
        this.weaponSlotLeft.GetComponent<SpriteRenderer>().sprite = this.inHands.icon;
        this.weaponSlotRight.GetComponent<SpriteRenderer>().sprite = this.inHands.icon;
    }

    //check which weapon is out and set accordingly
    void SetCurrentSlot(int x)
    {
        if (this.currentWeaponSlot == x)
        {
            // Pull out fists if you select the current weapon slot you're holding
            this.currentWeaponSlot = 0;
            this.inHands = this.fist;
            SetWeaponSlotSprites();
        }
        else
        {
            if (this.weaponSlots[x] != null)
            {
                //this should change weapon based on slot value x
                this.currentWeaponSlot = x;
                this.inHands = this.weaponSlots[this.currentWeaponSlot];
                SetWeaponSlotSprites();
            }
        }
    }

    void Attack()
    {
        if (player1)
        {
            if (Input.GetButtonDown("Fire1") && isAttacking != true)
            {
                if (this.inHands.type == 0)
                {
                    // Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
                    StartCoroutine(Stab());
                }
                else if (this.inHands.type == 1 && this.inHands.ammoLoaded > 0)
                {
                    // Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
                    StartCoroutine(Shoot());
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
                    StartCoroutine(Shoot());
                }
            }
        }
    }

    private IEnumerator Stab()
    {
        isAttacking = true;
        Vector2 stabLocation = Vector2.up * 200.0f;

        // all for left
        Vector2 startStabLocationLeft = weaponSlotLocationLeft;
        weaponSlotLeft.transform.localPosition = Vector3.Slerp(startStabLocationLeft, stabLocation, Time.deltaTime);
        weaponSlotLeft.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(inHands.attackSpeed);
        weaponSlotLeft.GetComponent<BoxCollider2D>().enabled = false;
        weaponSlotLeft.transform.localPosition = weaponSlotLocationLeft;

        // duplicating for right, should probably pass in as parameter
        Vector2 startStabLocationRight = weaponSlotLocationRight;
        weaponSlotRight.transform.localPosition = Vector3.Slerp(startStabLocationRight, stabLocation, Time.deltaTime);
        weaponSlotRight.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(inHands.attackSpeed);
        weaponSlotRight.GetComponent<BoxCollider2D>().enabled = false;
        weaponSlotRight.transform.localPosition = weaponSlotLocationRight;
        isAttacking = false;
    }

    private void Reload()
    {
        if (this.inHands.ammoPool > 0)
        {

            int ammoToLoad = this.inHands.magazineSize - this.inHands.ammoLoaded;
            if (ammoToLoad > 0)
            {
                this.inHands.ammoPool -= ammoToLoad;
                this.inHands.ammoLoaded = this.inHands.magazineSize;
                Debug.Log("Bullets left in pool: " + this.inHands.ammoPool);
                Debug.Log("Magazine full with " + this.inHands.magazineSize + " bullets");
            }
        }
        else
        {
            // no ammo, can't reload
        }
    }

    private IEnumerator Shoot()
    {
        if (player1 && this.inHands.ammoLoaded > 0)
        {
            isAttacking = true;
            this.inHands.ammoLoaded--;
            Debug.Log("Shots left: " + this.inHands.ammoLoaded);
            Vector2 gunLocation;
            if (player.GetComponent<PlayerControl>().lookingLeft)
            {
                gunLocation = player.GetComponent<PlayerControl>().weaponSlotLocationLeft;
            }
            else
            {
                gunLocation = player.GetComponent<PlayerControl>().weaponSlotLocationRight;
            }
            Vector2 mouseLocation = Input.mousePosition;
            GameObject Newbullet = Instantiate(bullet, gunLocation, Quaternion.identity);
            Newbullet.GetComponent<bulletMovement>().targetForward = this.gameObject.transform.rotation * player.GetComponent<PlayerControl>().diff;
            Newbullet.GetComponent<bulletMovement>().bulletDamage = inHands.damage;
            yield return new WaitForSeconds(inHands.attackSpeed);
            isAttacking = false;
        }
        // else
        // {
        //     Vector2 gunLocation = gunStart;
        //     GameObject Newbullet = Instantiate(bullet, gunLocation, Quaternion.identity);
        //     Newbullet.GetComponent<bulletMovement>().targetForward = this.gameObject.transform.rotation * Vector2.up;
        //     Newbullet.GetComponent<bulletMovement>().bulletDamage = inHands.damage;
        //     yield return new WaitForSeconds(inHands.attackSpeed);
        //     isAttacking = false;
        //     // float rotationZ = this.gameObject.transform.rotation.z;
        //     // // rotationZ = Mathf.Atan2(-stickInput.x, stickInput.y) * Mathf.Rad2Deg;
        //     // transform.rotation = Quaternion.Euler(0f, 0f, rotationZ -90);
        // }

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
                SetWeaponSlotSprites();
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
