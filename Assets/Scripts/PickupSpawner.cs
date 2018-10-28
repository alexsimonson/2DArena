using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public Weapon spawnedWeapon;
	public BasicPistol basicPistol;
	public BasicKnife basicKnife;
	public BasicKnife advancedKnife;
	//could I then do weapon = new Pistol() or something like that?  to set the base stats of the gun
	BoxCollider2D bc;

	// Use this for initialization
	void Start () {
		//we had it like this once and I thought it worked...
		basicPistol = new BasicPistol();
		basicKnife = new BasicKnife();
		//constructor for new items (nameOf, damage, attackSpeed, spriteName)
		advancedKnife = new BasicKnife("Advanced Dagger", 100, .6f, "AdvancedDagger");

		if(Random.Range(0.0f, 1.0f) < 0.5f){
			spawnedWeapon = basicPistol;
			gameObject.GetComponent<SpriteRenderer>().sprite = spawnedWeapon.icon;
			//this line is necessary because the gun should face right when spawned on ground, but also looking is weird without
			gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
			
		}else{
			spawnedWeapon = advancedKnife;
			gameObject.GetComponent<SpriteRenderer>().sprite = spawnedWeapon.icon;
		}
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		// Debug.Log(col.gameObject.name + " could pick up the " + spawnedWeapon.nameOf);
	}
	
}
