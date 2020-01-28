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
    }

    void UpdateUi () {
        Manager.playerUI.ToggleAmmoUI (inHands.type);
        Manager.playerUI.ReloadAmmoHud (inHands.ammoPool, inHands.ammoLoaded);
    }

    // Update is called once per frame
    void Update () {
        if(PlayerControl.hasControl){
            DetectInput ();
            Attack ();
            ThrowWeapon ();
        }
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
            Manager.weaponSystem.StartCoroutine(Reload ());
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
        Manager.sfxAudioSource.PlayOneShot(colObj.GetComponent<PickupSpawner>().pickupAudio);

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
        Manager.weaponSystem.inHands = Manager.weaponSystem.weaponSlots[Manager.weaponSystem.currentWeaponSlot];
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
                Manager.weaponSystem.inHands = Manager.weaponSystem.weaponSlots[x];
                SetWeaponSlotSprites ();
            }
        }

    }

    public static void Attack () {
        if (Input.GetButtonDown ("Fire1") && Manager.weaponSystem.isAttacking != true) {
            if (Manager.weaponSystem.inHands.type == 0) {
                Manager.weaponSystem.StartCoroutine (Stab ());
            } else if (Manager.weaponSystem.inHands.type == 1 && Manager.weaponSystem.inHands.ammoLoaded > 0) {
                Manager.weaponSystem.StartCoroutine (Shoot ());
            }
        }
    }

    public static IEnumerator Stab () {
        Manager.weaponSystem.isAttacking = true;
        Manager.player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
        if (Manager.leftHanded) {
            Vector2 startStabLocationLeft = Manager.weaponSystem.weaponSlotLocationLeft;
            Vector2 stabLocationLeft = new Vector2 (Manager.player.transform.position.x, Manager.player.transform.position.y + -200);
            Manager.weaponSystem.weaponSlotLeft.GetComponent<BoxCollider2D> ().enabled = true;
            Manager.weaponSystem.weaponSlotLeft.transform.localPosition = Vector2.Lerp (startStabLocationLeft, stabLocationLeft, Time.deltaTime);
        } else {

            Vector2 startStabLocationRight = Manager.weaponSystem.weaponSlotLocationRight;
            Vector2 stabLocationRight = new Vector2 (-Manager.player.transform.position.x, Manager.player.transform.position.y + -200);
            Manager.weaponSystem.weaponSlotRight.GetComponent<BoxCollider2D> ().enabled = true;
            Manager.weaponSystem.weaponSlotRight.transform.localPosition = Vector2.Lerp (startStabLocationRight, stabLocationRight, Time.deltaTime);
        }

        yield return new WaitForSeconds (Manager.weaponSystem.inHands.attackSpeed);
        if (Manager.leftHanded) {

            Manager.weaponSystem.weaponSlotLeft.GetComponent<BoxCollider2D> ().enabled = false;
            Manager.weaponSystem.weaponSlotLeft.transform.localPosition = Manager.weaponSystem.weaponSlotLocationLeft;
        } else {

            Manager.weaponSystem.weaponSlotRight.GetComponent<BoxCollider2D> ().enabled = false;
            Manager.weaponSystem.weaponSlotRight.transform.localPosition = Manager.weaponSystem.weaponSlotLocationRight;
        }
        Manager.weaponSystem.isAttacking = false;
    }

    public static IEnumerator Shoot () {
        if (Manager.weaponSystem.inHands.ammoLoaded > 0) {
            Manager.weaponSystem.isAttacking = true;
            Manager.sfxAudioSource.PlayOneShot(Manager.weaponSystem.inHands.weaponAudio);
            Manager.shotsFired++;
            Manager.playerUI.UpdateAmmoHud ();
            Manager.weaponSystem.inHands.ammoLoaded--;

            GameObject newBullet;
            if (Manager.leftHanded) {
                newBullet = Instantiate (Manager.weaponSystem.bullet, Manager.playerControl.weaponSlotLocationLeft, Quaternion.identity);
            } else {
                newBullet = Instantiate (Manager.weaponSystem.bullet, Manager.playerControl.weaponSlotLocationRight, Quaternion.identity);
            }
            newBullet.GetComponent<Rigidbody2D> ().AddForce (-Manager.player.gameObject.transform.up * 1000);
            newBullet.GetComponent<BulletMovement> ().bulletDamage = Manager.weaponSystem.inHands.damage;
            yield return new WaitForSeconds (Manager.weaponSystem.inHands.attackSpeed);
            Manager.weaponSystem.isAttacking = false;
        }
    }

    private IEnumerator Reload () {
        if (this.inHands.ammoPool > 0 && this.inHands.type == 1 && this.inHands.ammoLoaded != this.inHands.magazineSize) {
            Manager.sfxAudioSource.PlayOneShot(Manager.weaponSystem.inHands.reloadAudio);
            yield return new WaitForSeconds(Manager.weaponSystem.inHands.reloadAudio.length);
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

    static void ThrowWeapon () {
        if (Input.GetKeyDown (KeyCode.G)) {
            if (Manager.weaponSystem.inHands.nameOf != "Fists") {
                Manager.weaponSystem.weaponSlots[Manager.weaponSystem.currentWeaponSlot] = null;
                Manager.inventoryUI.inventorySlots[Manager.weaponSystem.currentWeaponSlot - 1].SetActive (false);
                SetCurrentSlot (0);
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