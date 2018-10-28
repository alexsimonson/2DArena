using UnityEngine;
using System.Collections;

public class BasicPistol : Weapon {

	public BasicPistol(){
		nameOf = "Basic Pistol";
		damage = 1000;
		type = 1;
		icon = Resources.Load<Sprite>("Sprites/ShitGunSpr");
		bullet = Resources.Load<Sprite>("Sprites/BulletSpr");
	}	
}