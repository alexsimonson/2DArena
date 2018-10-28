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

	public BasicKnife(string newName, int newDamage, float newAttackSpeed, string iconName){
		nameOf = newName;
		damage = newDamage;
		attackSpeed = newAttackSpeed;
		string iconPath = "Sprites/" + iconName;
		icon = Resources.Load<Sprite>(iconPath);
		bullet = null;
	}
	
}
