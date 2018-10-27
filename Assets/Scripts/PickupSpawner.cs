using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public Weapon spawningWeapon;
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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if(Random.Range(0.0f, 1.0f) < 0.5f){
			spawningWeapon = basicPistol;
			gameObject.GetComponent<SpriteRenderer>().sprite = basicPistol.icon;
		}else{
			spawningWeapon = basicKnife;
			gameObject.GetComponent<SpriteRenderer>().sprite = basicKnife.icon;
		}
		Debug.Log(col.gameObject.name + " could pick up the " + spawningWeapon.nameOf);
	}

	
}
