using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public Weapon spawnedWeapon;
	public BasicPistol basicPistol;
	public BasicKnife basicKnife;
	public BasicKnife advancedKnife;
	public BasicKnife shitAxe;
	//could I then do weapon = new Pistol() or something like that?  to set the base stats of the gun
	BoxCollider2D bc;

	// Use this for initialization
	void Start () {
		//we had it like this once and I thought it worked...
		basicPistol = new BasicPistol();
		basicKnife = new BasicKnife();
		//constructor for new items (nameOf, damage, attackSpeed, spriteName)
		advancedKnife = new BasicKnife("Advanced Dagger", 100, .6f, "AdvancedDagger");
		shitAxe = new BasicKnife("Shitty Axe", 35, .3f, "ShitAxe");
		float randomRange = Random.Range(0.0f, 1.0f);
		if(randomRange < 0.25f){
			spawnedWeapon = basicPistol;	
		}else if(randomRange >= 0.25f && randomRange < .5f){
			spawnedWeapon = advancedKnife;	
		}
		else if(randomRange >= 0.5f && randomRange < .75f){
			spawnedWeapon = shitAxe;
		}
		else if(randomRange >= .75f && randomRange < 1f){
			spawnedWeapon = basicKnife;
		}
		//this will rotate guns appropriately every time
		if(spawnedWeapon.type==1){
			gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
		}
		//this will ensure the sprite is changed every time
		gameObject.GetComponent<SpriteRenderer>().sprite = spawnedWeapon.icon;
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		// Debug.Log(col.gameObject.name + " could pick up the " + spawnedWeapon.nameOf);
	}
	
}
