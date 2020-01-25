using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {
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
    private int activeWeaponSlot = 0;
    private bool isAttacking = false;

    public static bool IsLeft, IsRight, IsUp, IsDown;
    private float _LastX, _LastY;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        bc = GetComponent<BoxCollider2D> ();
        weaponSlots = new Weapon[4];
        weaponSlots[0] = new Fist ();
        inHands = weaponSlots[0];

        weaponSlotLeft.GetComponent<SpriteRenderer> ().sortingLayerName = "Weapons";
        weaponSlotRight.GetComponent<SpriteRenderer> ().sortingLayerName = "Weapons";

        weaponSlotLocationLeft = weaponSlotLeft.transform.localPosition;
        weaponSlotLocationRight = weaponSlotRight.transform.localPosition;

        SetWeaponSlotSprites ();
        // UpdateUi();
    }

    void UpdateUi () {
        Manager.playerUI.ToggleAmmoUI (inHands.type);
        Manager.playerUI.ReloadAmmoHud (inHands.ammoPool, inHands.ammoLoaded);
    }

    // Update is called once per frame
    void Update () {
        DetectInput ();
        Attack ();
        ThrowWeapon ();
    }

    void ToggleAmmoUI () {
        if (this.inHands.type == 0) {
            Manager.playerUI.ToggleAmmoUI (this.inHands.type);
        } else {
            Manager.playerUI.ToggleAmmoUI (this.inHands.type);
            Manager.playerUI.ReloadAmmoHud (this.inHands.ammoPool, this.inHands.ammoLoaded);
        }
    }

    void DetectInput () {
        // manage Dpad input as a button
        float x = Input.GetAxis ("CycleInventory");
        float y = Input.GetAxis ("Dpad Y");
        IsLeft = false;
        IsRight = false;
        IsUp = false;
        IsDown = false;
        if (_LastX != x) {
            if (x == -1)
                IsLeft = true;
            else if (x == 1)
                IsRight = true;
        }
        if (_LastY != y) {
            if (y == -1)
                IsDown = true;
            else if (y == 1)
                IsUp = true;
        }
        // end manage Dpad input as a button
        _LastX = x;
        _LastY = y;
        if (Input.GetButtonDown ("1")) {
            SetCurrentSlot (1);
        }
        if (Input.GetButtonDown ("2")) {
            SetCurrentSlot (2);
        }
        if (Input.GetButtonDown ("3")) {
            SetCurrentSlot (3);
        }
        if (Input.GetButtonDown ("Interact") || Input.GetButtonDown ("InteractController")) {
            InteractWith (this.interactInRange);
        }
        if (Input.GetButtonDown ("Reload") || Input.GetButtonDown ("ReloadController")) {
            Reload ();
        }
        if (IsLeft) {
            int setSlot = 0;
            if (activeWeaponSlot == 1) {
                if (weaponSlots[3] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 3;
                    setSlot = 3;
                } else if (weaponSlots[2] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 2;
                    setSlot = 2;
                }
            } else if (activeWeaponSlot == 2) {
                if (weaponSlots[1] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 1;
                    setSlot = 1;
                } else if (weaponSlots[3] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 3;
                    setSlot = 3;
                }
            } else if (activeWeaponSlot == 3) {
                if (weaponSlots[2] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 2;
                    setSlot = 2;
                } else if (weaponSlots[1] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 1;
                    setSlot = 1;
                }
            }
            if (currentWeaponSlot != 0 && setSlot != 0) {
                SetCurrentSlot (setSlot);
            }
        }

        if (IsRight) {
            int setSlot = 0;
            if (activeWeaponSlot == 3) {
                if (weaponSlots[1] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 1;
                    setSlot = 1;
                } else if (weaponSlots[2] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 2;
                    setSlot = 2;
                }
            } else if (activeWeaponSlot == 2) {
                if (weaponSlots[3] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 3;
                    setSlot = 3;
                } else if (weaponSlots[1] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 1;
                    setSlot = 1;
                }
            } else if (activeWeaponSlot == 1) {
                if (weaponSlots[2] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 2;
                    setSlot = 2;
                } else if (weaponSlots[3] != null) {
                    Manager.weaponSystem.activeWeaponSlot = 3;
                    setSlot = 3;
                }
            }
            if (currentWeaponSlot != 0 && setSlot != 0) {
                SetCurrentSlot (setSlot);
            }
        }

        if (IsUp || IsDown) {
            Debug.Log ("Active WeaponSlot: " + activeWeaponSlot);
            if (currentWeaponSlot == 0) {
                if (weaponSlots[activeWeaponSlot] != null) {
                    SetCurrentSlot (activeWeaponSlot);
                }
            } else {
                SetCurrentSlot (0);
            }
        }
    }

    public static void PickupWeapon (GameObject colObj) {
        Weapon spawnedWeapon = colObj.GetComponent<PickupSpawner> ().spawnedWeapon;
        foreach (Weapon weapon in Manager.weaponSystem.weaponSlots) {
            if (weapon != null && weapon.nameOf == spawnedWeapon.nameOf) {
                // just add ammo
                weapon.ammoPool += spawnedWeapon.addAmmo;
                Manager.playerUI.SetAmmoReserve (weapon.ammoPool);
                return;
            }
        }

        if (Manager.weaponSystem.weaponSlots[1] == null) {
            Manager.weaponSystem.activeWeaponSlot = 1;
            Manager.weaponSystem.currentWeaponSlot = 1;
        } else if (Manager.weaponSystem.weaponSlots[2] == null) {
            Manager.weaponSystem.activeWeaponSlot = 2;
            Manager.weaponSystem.currentWeaponSlot = 2;
        } else if (Manager.weaponSystem.weaponSlots[3] == null) {
            Manager.weaponSystem.activeWeaponSlot = 3;
            Manager.weaponSystem.currentWeaponSlot = 3;
        }

        Manager.weaponSystem.weaponSlots[Manager.weaponSystem.currentWeaponSlot] = spawnedWeapon;
        Manager.weaponSystem.inHands = spawnedWeapon;
        Manager.playerUI.ReloadAmmoHud (Manager.weaponSystem.inHands.ammoPool, Manager.weaponSystem.inHands.ammoLoaded);
        Manager.inventoryUI.UpdateImage (Manager.weaponSystem.currentWeaponSlot, Manager.weaponSystem.inHands.icon);
        SetWeaponSlotSprites ();
    }

    public static void SetWeaponSlotSprites () {
        if (Manager.weaponSystem.inHands.nameOf == "Fists") {
            Manager.weaponSystem.weaponSlotLeft.GetComponent<SpriteRenderer> ().sprite = null;
            Manager.weaponSystem.weaponSlotRight.GetComponent<SpriteRenderer> ().sprite = null;
        } else {
            Manager.weaponSystem.weaponSlotLeft.GetComponent<SpriteRenderer> ().sprite = Manager.weaponSystem.inHands.icon;
            Manager.weaponSystem.weaponSlotRight.GetComponent<SpriteRenderer> ().sprite = Manager.weaponSystem.inHands.icon;
        }
        Manager.weaponSystem.UpdateUi ();
    }

    //check which weapon is out and set accordingly
    static void SetCurrentSlot (int x) {
        if (Manager.weaponSystem.currentWeaponSlot == x) {
            // Pull out fists if you select the current weapon slot you're holding
            Manager.weaponSystem.currentWeaponSlot = 0;
            Manager.weaponSystem.inHands = Manager.weaponSystem.weaponSlots[0];
            SetWeaponSlotSprites ();
        } else {
            if (Manager.weaponSystem.weaponSlots[x] != null) {
                //Manager.weaponSystem should change weapon based on slot value x
                Manager.weaponSystem.currentWeaponSlot = x;
                Manager.weaponSystem.inHands = Manager.weaponSystem.weaponSlots[Manager.weaponSystem.currentWeaponSlot];
                SetWeaponSlotSprites ();
            }
        }

    }

    void Attack () {
        if (Input.GetButtonDown ("Fire1") && isAttacking != true) {
            if (this.inHands.type == 0) {
                StartCoroutine (Stab ());
            } else if (this.inHands.type == 1 && this.inHands.ammoLoaded > 0) {
                StartCoroutine (Shoot ());
            }
        }
    }

    private IEnumerator Stab () {
        isAttacking = true;
        Manager.player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
        if (Manager.leftHanded) {
            Vector2 startStabLocationLeft = weaponSlotLocationLeft;
            Vector2 stabLocationLeft = new Vector2 (Manager.player.transform.position.x, Manager.player.transform.position.y + -200);
            weaponSlotLeft.GetComponent<BoxCollider2D> ().enabled = true;
            weaponSlotLeft.transform.localPosition = Vector2.Lerp (startStabLocationLeft, stabLocationLeft, Time.deltaTime);
        } else {

            Vector2 startStabLocationRight = weaponSlotLocationRight;
            Vector2 stabLocationRight = new Vector2 (-Manager.player.transform.position.x, Manager.player.transform.position.y + -200);
            weaponSlotRight.GetComponent<BoxCollider2D> ().enabled = true;
            weaponSlotRight.transform.localPosition = Vector2.Lerp (startStabLocationRight, stabLocationRight, Time.deltaTime);
        }

        yield return new WaitForSeconds (inHands.attackSpeed);
        if (Manager.leftHanded) {

            weaponSlotLeft.GetComponent<BoxCollider2D> ().enabled = false;
            weaponSlotLeft.transform.localPosition = weaponSlotLocationLeft;
        } else {

            weaponSlotRight.GetComponent<BoxCollider2D> ().enabled = false;
            weaponSlotRight.transform.localPosition = weaponSlotLocationRight;
        }
        isAttacking = false;
    }

    private void Reload () {
        if (this.inHands.ammoPool > 0) {
            int ammoToLoad = this.inHands.magazineSize - this.inHands.ammoLoaded;
            if (ammoToLoad > 0) {
                if (this.inHands.ammoPool - ammoToLoad >= 0) {
                    this.inHands.ammoPool -= ammoToLoad;
                    this.inHands.ammoLoaded = this.inHands.magazineSize;
                } else {
                    // load the rest of the ammoPool
                    this.inHands.ammoLoaded += this.inHands.ammoPool;
                    this.inHands.ammoPool = 0;
                }
                Manager.playerUI.ReloadAmmoHud (this.inHands.ammoPool, this.inHands.ammoLoaded);
            }
        } else {
            // no ammo, can't reload
        }
    }

    private IEnumerator Shoot () {
        if (this.inHands.ammoLoaded > 0) {
            isAttacking = true;
            Manager.shotsFired++;
            Manager.playerUI.UpdateAmmoHud ();
            this.inHands.ammoLoaded--;
            Vector2 leftGunLocation = Manager.playerControl.weaponSlotLocationLeft;
            Vector2 rightGunLocation = Manager.playerControl.weaponSlotLocationRight;

            Vector2 mouseLocation = Input.mousePosition;
            GameObject newBullet;
            if (Manager.leftHanded) {
                newBullet = Instantiate (bullet, leftGunLocation, Quaternion.identity);
            } else {
                newBullet = Instantiate (bullet, rightGunLocation, Quaternion.identity);
            }
            newBullet.GetComponent<Rigidbody2D> ().AddForce (-transform.up * 1000);
            newBullet.GetComponent<BulletMovement> ().bulletDamage = inHands.damage;
            yield return new WaitForSeconds (inHands.attackSpeed);
            isAttacking = false;
        }
    }

    static void ThrowWeapon () {
        if (Input.GetKeyDown (KeyCode.G)) {
            Debug.Log (Manager.inventoryUI.inventorySlots);
            Debug.Log (Manager.inventoryUI.inventorySlots[0]);
            Debug.Log (Manager.inventoryUI.inventorySlots[1]);
            Debug.Log (Manager.inventoryUI.inventorySlots[2]);
            Debug.Log ("Current weapon slot: " + Manager.weaponSystem.currentWeaponSlot);
            Debug.Log (Manager.weaponSystem.inHands);
            Debug.Log (Manager.weaponSystem.inHands.nameOf);
            Debug.Log (Manager.weaponSystem.inHands.type);
            if (Manager.weaponSystem.inHands.nameOf != "Fists") {
                Manager.weaponSystem.weaponSlots[Manager.weaponSystem.currentWeaponSlot] = null;
                Manager.inventoryUI.inventorySlots[Manager.weaponSystem.currentWeaponSlot - 1].SetActive (false);
                SetCurrentSlot (0);
                // Manager.weaponSystem.inHands = Manager.weaponSystem.fist;
                Manager.playerUI.ToggleAmmoUI (Manager.weaponSystem.inHands.type);
                SetWeaponSlotSprites ();
            }
        }
    }

    void InteractWith (GameObject colObj) {
        if (colObj != null) {
            if (colObj.tag == "Pickup") {
                PickupWeapon (colObj);
                UpdateUi ();
            } else {
                // Debug.Log("NO Pickup AVAILABLE");
            }
        }
    }

    void OnTriggerExit2D (Collider2D col) {
        interactInRange = null;
    }

    void OnTriggerStay2D (Collider2D col) {
        interactInRange = col.gameObject;
    }

}