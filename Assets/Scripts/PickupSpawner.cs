using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public Weapon spawningWeapon;

	//considering trying this?
	// public BasicPistol basicPistol = new BasicPistol();
	// public BasicKnife basicKnife = new BasicKnife();

	public BasicPistol basicPistol;
	public BasicKnife basicKnife;

	public Sprite pistolSpr;
	public Sprite knifeSpr;
	
	//could I then do weapon = new Pistol() or something like that?  to set the base stats of the gun
	BoxCollider2D bc;

	// Use this for initialization
	void Start () {
		//we had it like this once and I thought it worked...
		basicPistol = new BasicPistol();
		basicKnife = new BasicKnife();
		if(Random.Range(0, 1) < .5){
			spawningWeapon = basicPistol;
			gameObject.GetComponent<SpriteRenderer>().sprite = pistolSpr;
		}else{
			spawningWeapon = basicKnife;
			gameObject.GetComponent<SpriteRenderer>().sprite = knifeSpr;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log(col.gameObject.name + " could pick up the " + spawningWeapon.nameOf);
	}

	
}
