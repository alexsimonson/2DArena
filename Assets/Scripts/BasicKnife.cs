using UnityEngine;
using System.Collections;

public class BasicKnife : Weapon {

	public BasicKnife(){
		nameOf = "Basic Knife";
		damage = 50;
		type = 0;
		attackSpeed = .3f;
		icon = Resources.Load<Sprite>("Sprites/ShitDaggerSpr");
		bullet = null;
	}
	
}
