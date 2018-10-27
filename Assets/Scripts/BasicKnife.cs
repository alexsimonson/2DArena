using UnityEngine;
using System.Collections;

public class BasicKnife : Weapon {

	public BasicKnife(){
		nameOf = "Basic Knife";
		damage = 10;
		type = 0;
		icon = Resources.Load<Sprite>("Sprites/ShitDaggerSpr");
	}
	
}
