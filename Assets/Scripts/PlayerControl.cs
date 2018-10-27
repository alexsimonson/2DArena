using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private float walkSpeed = 4f;

	private Rigidbody2D rb;
	private BoxCollider2D bc;

	public Weapon inHands;
	public Fist fist;

	private GameObject interactInRange = null;

	private GameObject weaponSlot;
	private Vector2 weaponSlotLocation;

	private bool isAttacking = false;
	
	// Use this for initialization
	void Start () {
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
	void Update () {
		MoveDirection();
		LookRotation();
		Attack();
		InteractWith(interactInRange);
		if(Input.GetKeyDown(KeyCode.G)){
			ThrowWeapon();
		}
	}

	void MoveDirection(){
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity=targetVelocity * walkSpeed;
	}

	void LookRotation(){
		// Vector2 playerLocation = gameObject.transform.position;
		// Vector2 mouseLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		// Vector2 direction = (playerLocation-mouseLocation);

		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
		float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotationZ -90);
	}

	void Attack(){
		Debug.Log("Is Attacking: " + isAttacking);
		if(Input.GetButtonDown("Fire1") && isAttacking != true){
			//this should determine what type of weapon is being used and then attack accordingly
			Debug.Log("Attack");
			isAttacking = true;

			if(inHands.type==0){
				Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
				StartCoroutine(Stab());
				
			}else if(inHands.type==1){
				Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
				StartCoroutine(Shoot());
			}

		}
		
	}

	private IEnumerator Stab(){
			Vector2 stabLocation = Vector2.up * 200.0f;
			Vector2 startStabLocation = weaponSlotLocation;

			weaponSlot.transform.localPosition = Vector3.Slerp(startStabLocation, stabLocation, Time.deltaTime);

			yield return new WaitForSeconds(0.2f);
			Debug.Log("this is happening");

			weaponSlot.transform.localPosition = weaponSlotLocation;
			isAttacking = false;
			//weaponSlot.transform.position = weaponSlotLocation;

		
	}

	private IEnumerator Shoot(){
			// Vector2 stabLocation = Vector2.up * 200.0f;
			// Vector2 startStabLocation = weaponSlotLocation;

			// weaponSlot.transform.localPosition = Vector3.Slerp(startStabLocation, stabLocation, Time.deltaTime);

			yield return new WaitForSeconds(0.2f);
			// Debug.Log("this is happening");

			// weaponSlot.transform.localPosition = weaponSlotLocation;
			isAttacking = false;
			//weaponSlot.transform.position = weaponSlotLocation;

		
	}

	void ThrowWeapon(){
		if(inHands.nameOf=="Fists"){
			Debug.Log("You can't throw your fist");
		}else{
			//this should spawn a game object spawner essentially in the game
			Debug.Log("Throwing weapon: " + inHands.nameOf);
			inHands=fist;
			weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
			Debug.Log("Now you're left with your fists");
		}
	}

	void InteractWith(GameObject colObj){
		if(Input.GetKeyDown(KeyCode.E)){
			if(colObj!=null){
				if(colObj.tag=="Pickup"){
					PickupWeapon(colObj);
				}else{
					Debug.Log("NO Pickup AVAILABLE");
				}

			}else{
				Debug.Log("Nothing in range");
			}
		}
	}

	void PickupWeapon(GameObject colObj){
		Weapon sw = colObj.GetComponent<PickupSpawner>().spawnedWeapon;
		Debug.Log("Swapping " + inHands.nameOf + " for " + sw.nameOf);
		inHands = sw;
		weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
		Debug.Log("Current equipped weapon: " + inHands.nameOf);

	}
	
	void OnTriggerEnter2D(Collider2D col){
		
	}

	void OnTriggerExit2D(Collider2D col){
		interactInRange = null;
	}

	void OnTriggerStay2D(Collider2D col){
		interactInRange = col.gameObject;

		// InteractWith(colObj);
		// if(Input.GetKeyDown(KeyCode.E)){
		// 	if(colObj.tag=="Pickup"){
		// 		Debug.Log("Picking up object");
		// 		PickupGun(colObj);
		// 	}
		// }
	}
}
