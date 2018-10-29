using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

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
		if(hasControl){
			MoveDirection();
			LookRotation();
			Attack();
			InteractWith(interactInRange);
			ThrowWeapon();			
		}
	}

	void MoveDirection(){
		if (player1){
			Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        	rb.velocity=targetVelocity * walkSpeed;
		}
		else if (player1 == false){
			Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("XboxHorizontal"), Input.GetAxisRaw("XboxVertical"));
        	rb.velocity=targetVelocity * walkSpeed;
		}
	}

	void LookRotation(){
		if (player1){
			Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        	diff.Normalize();
			float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rotationZ -90);
		}
		else if (player1 == false){
			float inputHoriz = Input.GetAxisRaw("XboxStickHorizontal");
			float inputVert =  Input.GetAxisRaw("XboxStickVertical");
			float deadzone = 0.25f;
			Vector2 stickInput = new Vector2(Input.GetAxisRaw("XboxStickVertical"), Input.GetAxisRaw("XboxStickHorizontal"));
			if(stickInput.magnitude < deadzone)
				stickInput = Vector2.zero;
			else {
				float rotationZ = Mathf.Atan2(-stickInput.x, stickInput.y) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, rotationZ -90);
			}
			
			
			
		}
	}

	void Attack(){
		if (player1){
			if(Input.GetButtonDown("Fire1") && isAttacking != true){
			isAttacking = true;
			if(inHands.type==0){
				// Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
				StartCoroutine(Stab());
			}else if(inHands.type==1){
				// Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
				StartCoroutine(Shoot(gameObject.transform.position));
			}
		}
		}
		else {
			
			if(Input.GetButtonDown("XboxFire1") && isAttacking != true){
				Debug.Log("XboxFire1 is happeneing");
			isAttacking = true;
			if(inHands.type==0){
				// Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
				StartCoroutine(Stab());
			}else if(inHands.type==1){
				// Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
				StartCoroutine(Shoot(gameObject.transform.position));
			}
			}
		}
	}

	private IEnumerator Stab(){
		Vector2 stabLocation = Vector2.up * 200.0f;
		Vector2 startStabLocation = weaponSlotLocation;
		weaponSlot.transform.localPosition = Vector3.Slerp(startStabLocation, stabLocation, Time.deltaTime);
		weaponSlot.GetComponent<BoxCollider2D>().enabled = true;
		yield return new WaitForSeconds(inHands.attackSpeed);
		weaponSlot.GetComponent<BoxCollider2D>().enabled = false;
		weaponSlot.transform.localPosition = weaponSlotLocation;
		isAttacking = false;
	}

	private IEnumerator Shoot(Vector2 gunStart){
		if (player1){
			Vector2 gunLocation = gunStart;
			Vector2 mouseLocation = Input.mousePosition;
			GameObject Newbullet =Instantiate(bullet, gunLocation, Quaternion.identity);
			Newbullet.GetComponent<bulletMovement>().targetForward = this.gameObject.transform.rotation * Vector2.up;
			Newbullet.GetComponent<bulletMovement>().bulletDamage = inHands.damage;
			yield return new WaitForSeconds(inHands.attackSpeed);		
			isAttacking = false;
		}
		else {
			Vector2 gunLocation = gunStart;
			GameObject Newbullet =Instantiate(bullet, gunLocation, Quaternion.identity);
			Newbullet.GetComponent<bulletMovement>().targetForward = this.gameObject.transform.rotation * Vector2.up;
			Newbullet.GetComponent<bulletMovement>().bulletDamage = inHands.damage;
			yield return new WaitForSeconds(inHands.attackSpeed);		
			isAttacking = false;
			// float rotationZ = this.gameObject.transform.rotation.z;
			// // rotationZ = Mathf.Atan2(-stickInput.x, stickInput.y) * Mathf.Rad2Deg;
			// transform.rotation = Quaternion.Euler(0f, 0f, rotationZ -90);
		}
		
	}

	void ThrowWeapon(){
		if(Input.GetKeyDown(KeyCode.G)){	
			if(inHands.nameOf=="Fists"){
				// Debug.Log("You can't throw your fist");
			}else{
				//this should spawn a game object spawner essentially in the game
				// Debug.Log("Throwing weapon: " + inHands.nameOf);
				inHands=fist;
				weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
			}
		}
	}

	void InteractWith(GameObject colObj){
		if (player1){
			if(Input.GetKeyDown(KeyCode.E)){
				if(colObj!=null){
					if(colObj.tag=="Pickup"){
						PickupWeapon(colObj);
					}else{
						// Debug.Log("NO Pickup AVAILABLE");
					}
				}else{
					// Debug.Log("Nothing in range");
				}
			}
		}
		else {
			if (Input.GetButtonDown("XboxPickup")){
				Debug.Log("I AM PICKUP");
				if(colObj!=null){
					if(colObj.tag=="Pickup"){
						PickupWeapon(colObj);
					}else{
						// Debug.Log("NO Pickup AVAILABLE");
					}
				}else{
					// Debug.Log("Nothing in range");
				}	
			}
		}
	}

	void PickupWeapon(GameObject colObj){
		Weapon sw = colObj.GetComponent<PickupSpawner>().spawnedWeapon;
		// Debug.Log("Swapping " + inHands.nameOf + " for " + sw.nameOf);
		inHands = sw;
		weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
		// Debug.Log("Current equipped weapon: " + inHands.nameOf);

	}
	
	void OnTriggerEnter2D(Collider2D col){
		
	}

	void OnTriggerExit2D(Collider2D col){
		interactInRange = null;
	}

	void OnTriggerStay2D(Collider2D col){
		interactInRange = col.gameObject;
	}
}
