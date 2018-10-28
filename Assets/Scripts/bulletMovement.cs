using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMovement : MonoBehaviour {
	Rigidbody2D rb;
	GameObject player;
	Vector2 targetForward;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		targetForward = player.transform.rotation * Vector2.up;
	}
	
	// Update is called once per frame
	void Update () {
		// rb.transform.Translate = tra

		rb.transform.Translate(targetForward*10*Time.deltaTime);
	}
}
