using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private float walkSpeed = 4f;

	private Rigidbody2D rb2d;
	
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		MoveDirection();
		LookRotation();
	}

	void MoveDirection(){
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2d.velocity=targetVelocity * walkSpeed;
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
}
