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
		rb.transform.Translate(targetForward*10*Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag=="Collider"){
			// Debug.Log("Bullet hit wall");
			Destroy(gameObject);
		}else if(col.tag=="Enemy" || col.tag=="EnemySpawner"){
			// Debug.Log("Bullet hit enemy");
			//this needs to apply damage from the weapon
			col.GetComponent<Health>().TakeDamage(player.GetComponent<PlayerControl>().inHands.damage);
			Destroy(gameObject);
		}
	}
}
