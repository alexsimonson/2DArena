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
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
		fist = new Fist();
		inHands = fist;
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
		if(Input.GetButtonDown("Fire1")){
			//this should determine what type of weapon is being used and then attack accordingly
			Debug.Log("Attack");
			if(inHands.type==0){
				Debug.Log("Attacking with a stab weapon: " + inHands.nameOf);
			}else if(inHands.type==1){
				Debug.Log("Attacking with a shoot weapon: " + inHands.nameOf);
			}

		}
	}

	void ThrowWeapon(){
		if(inHands.nameOf=="Fists"){
			Debug.Log("You can't throw your fist");
		}else{
			//this should spawn a game object spawner essentially in the game
			Debug.Log("Throwing weapon: " + inHands.nameOf);
			inHands=fist;
			Debug.Log("Now you're left with your fists");
		}
	}

	void InteractWith(GameObject colObj){
		if(Input.GetKeyDown(KeyCode.E)){
			Debug.Log("E PRESSED");	
			if(colObj.tag=="Pickup"){
				PickupGun(colObj);
			}else{
				Debug.Log("NO Pickup AVAILABLE");
			}
		// 	if(interactInRange!=null){
		// 		Debug.Log("Tag of thing: " + interactInRange.gameObject.tag);

		// 	}else{
		// 		Debug.Log("Nothing in range to pickup");
		// 	}
		}
	}

	void PickupGun(GameObject colObj){
		Weapon sw = colObj.GetComponent<PickupSpawner>().spawnedWeapon;
		Debug.Log("Swapping " + inHands.nameOf + " for " + sw.nameOf);
		inHands = sw;
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
