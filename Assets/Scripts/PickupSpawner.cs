using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public Weapon spawnedWeapon;
	public BasicPistol basicPistol;
	public BasicKnife basicKnife;
	
	//could I then do weapon = new Pistol() or something like that?  to set the base stats of the gun
	BoxCollider2D bc;

	// Use this for initialization
	void Start () {
		//we had it like this once and I thought it worked...
		basicPistol = new BasicPistol();
		basicKnife = new BasicKnife();
		if(Random.Range(0.0f, 1.0f) < 0.5f){
			spawnedWeapon = basicPistol;
			gameObject.GetComponent<SpriteRenderer>().sprite = basicPistol.icon;
		}else{
			spawnedWeapon = basicKnife;
			gameObject.GetComponent<SpriteRenderer>().sprite = basicKnife.icon;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){

		// if(col.gameObject.tag=="Player"){
			
		// }


		if(Random.Range(0.0f, 1.0f) < 0.5f){
			spawnedWeapon = basicPistol;
			gameObject.GetComponent<SpriteRenderer>().sprite = basicPistol.icon;
		}else{
			spawnedWeapon = basicKnife;
			gameObject.GetComponent<SpriteRenderer>().sprite = basicKnife.icon;
		}
		Debug.Log(col.gameObject.name + " could pick up the " + spawnedWeapon.nameOf);
	}

	public void SleepSpawner(){
		Debug.Log("Sleeping");
		
	}

	
}
