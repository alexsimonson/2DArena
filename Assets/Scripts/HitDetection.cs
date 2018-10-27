using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour {

	private BoxCollider2D bc;
	private GameObject self;
	// Use this for initialization
	void Start () {
		bc = gameObject.GetComponent<BoxCollider2D>();
		self = gameObject;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(self.tag=="Player"){
			if(col.gameObject.tag=="Enemy"){
				Debug.Log("Hitting an enemy");
				col.gameObject.GetComponent<Health>().TakeDamage(self.GetComponentInParent<PlayerControl>().inHands.damage);
			}
		}else if(self.tag=="Enemy"){
			if(col.gameObject.tag=="Player"){
				col.gameObject.GetComponent<Health>().TakeDamage(self.GetComponentInParent<AiControl>().inHands.damage);
			}
		}
	}
}
