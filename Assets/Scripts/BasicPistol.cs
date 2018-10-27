using UnityEngine;
using System.Collections;

public class BasicPistol : Weapon {

	public BasicPistol(){
		nameOf = "Basic Pistol";
		damage = 10;
		type = 1;
		icon = Resources.Load<Sprite>("Sprites/BasicGunSpr");
	}
	
}
