using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // initializing gameobject references
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

    private int currentWeaponSlot = 0;
    private bool isAttacking = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        fist = new Fist();
        Manager.weaponSystem.inHands = fist;
        Manager.weaponSystem.weaponSlots = new Weapon[4];

        weaponSlotLeft.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
        weaponSlotRight.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";

        weaponSlotLocationLeft = weaponSlotLeft.transform.localPosition;
        weaponSlotLocationRight = weaponSlotRight.transform.localPosition;

        SetWeaponSlotSprites();
        UpdateUi();
    }

    void UpdateUi()
    {
        Manager.playerUI.ToggleAmmoUI(this.inHands.type);
        Manager.playerUI.ReloadAmmoHud(this.inHands.ammoPool, this.inHands.ammoLoaded);
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        Attack();
        ThrowWeapon();
    }

    void ToggleAmmoUI()
    {

        if (this.inHands.type == 0)
        {
            Manager.playerUI.ToggleAmmoUI(this.inHands.type);
        }
        else
        {
            Manager.playerUI.ToggleAmmoUI(this.inHands.type);
            Manager.playerUI.ReloadAmmoHud(this.inHands.ammoPool, this.inHands.ammoLoaded);
        }
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
        Weapon spawnedWeapon = colObj.GetComponent<PickupSpawner>().spawnedWeapon;
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != null && weapon.nameOf == spawnedWeapon.nameOf)
            {
                // just add ammo
                weapon.ammoPool += spawnedWeapon.addAmmo;
                Manager.playerUI.SetAmmoReserve(weapon.ammoPool);
                return;
            }
        }

        if (this.weaponSlots[1] == null)
        {
            this.currentWeaponSlot = 1;
        }
        else if (this.weaponSlots[2] == null)
        {
            this.currentWeaponSlot = 2;
        }
        else if (this.weaponSlots[3] == null)
        {
            this.currentWeaponSlot = 3;
        }

        this.weaponSlots[this.currentWeaponSlot] = spawnedWeapon;
        this.inHands = spawnedWeapon;
        Manager.playerUI.ReloadAmmoHud(this.inHands.ammoPool, this.inHands.ammoLoaded);
        Manager.inventoryUI.UpdateImage(this.currentWeaponSlot, this.inHands.icon);
        SetWeaponSlotSprites();
    }

    public void SetWeaponSlotSprites()
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
        UpdateUi();
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && isAttacking != true)
        {
            if (this.inHands.type == 0)
            {
                StartCoroutine(Stab());
            }
            else if (this.inHands.type == 1 && this.inHands.ammoLoaded > 0)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Stab()
    {
        isAttacking = true;
        Manager.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (Manager.leftHanded)
        {
            Vector2 startStabLocationLeft = weaponSlotLocationLeft;
            Vector2 stabLocationLeft = new Vector2(Manager.player.transform.position.x, Manager.player.transform.position.y + -200);
            weaponSlotLeft.GetComponent<BoxCollider2D>().enabled = true;
            weaponSlotLeft.transform.localPosition = Vector2.Lerp(startStabLocationLeft, stabLocationLeft, Time.deltaTime);
        }
        else
        {

            Vector2 startStabLocationRight = weaponSlotLocationRight;
            Vector2 stabLocationRight = new Vector2(-Manager.player.transform.position.x, Manager.player.transform.position.y + -200);
            weaponSlotRight.GetComponent<BoxCollider2D>().enabled = true;
            weaponSlotRight.transform.localPosition = Vector2.Lerp(startStabLocationRight, stabLocationRight, Time.deltaTime);
        }

        yield return new WaitForSeconds(inHands.attackSpeed);
        if (Manager.leftHanded)
        {

            weaponSlotLeft.GetComponent<BoxCollider2D>().enabled = false;
            weaponSlotLeft.transform.localPosition = weaponSlotLocationLeft;
        }
        else
        {

            weaponSlotRight.GetComponent<BoxCollider2D>().enabled = false;
            weaponSlotRight.transform.localPosition = weaponSlotLocationRight;
        }
        isAttacking = false;
    }

    private void Reload()
    {
        if (this.inHands.ammoPool > 0)
        {
            int ammoToLoad = this.inHands.magazineSize - this.inHands.ammoLoaded;
            if (ammoToLoad > 0)
            {
                if (this.inHands.ammoPool - ammoToLoad >= 0)
                {
                    this.inHands.ammoPool -= ammoToLoad;
                    this.inHands.ammoLoaded = this.inHands.magazineSize;
                }
                else
                {
                    // load the rest of the ammoPool
                    this.inHands.ammoLoaded += this.inHands.ammoPool;
                    this.inHands.ammoPool = 0;
                }
                Manager.playerUI.ReloadAmmoHud(this.inHands.ammoPool, this.inHands.ammoLoaded);
            }
        }
        else
        {
            // no ammo, can't reload
        }
    }

    private IEnumerator Shoot()
    {
        if (this.inHands.ammoLoaded > 0)
        {
            isAttacking = true;
            Manager.shotsFired++;
            Manager.playerUI.UpdateAmmoHud();
            this.inHands.ammoLoaded--;
            Vector2 leftGunLocation = Manager.playerControl.weaponSlotLocationLeft;
            Vector2 rightGunLocation = Manager.playerControl.weaponSlotLocationRight;

            Vector2 mouseLocation = Input.mousePosition;
            GameObject newBullet;
            if (Manager.leftHanded)
            {
                newBullet = Instantiate(bullet, leftGunLocation, Quaternion.identity);
            }
            else
            {
                newBullet = Instantiate(bullet, rightGunLocation, Quaternion.identity);
            }
            newBullet.GetComponent<Rigidbody2D>().AddForce(-transform.up * 1000);
            newBullet.GetComponent<BulletMovement>().bulletDamage = inHands.damage;
            yield return new WaitForSeconds(inHands.attackSpeed);
            isAttacking = false;
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
                Manager.inventoryUI.inventorySlots[this.currentWeaponSlot].SetActive(false);
                Manager.playerUI.ToggleAmmoUI(this.inHands.type);
                SetWeaponSlotSprites();
            }
        }
    }

    void InteractWith(GameObject colObj)
    {
        if (colObj != null)
        {
            if (colObj.tag == "Pickup")
            {
                PickupWeapon(colObj);
                UpdateUi();
            }
            else
            {
                // Debug.Log("NO Pickup AVAILABLE");
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
