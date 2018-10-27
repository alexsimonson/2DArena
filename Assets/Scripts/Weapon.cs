using UnityEngine;
using System.Collections;

public class Weapon {
	[SerializeField] public string nameOf;
	[SerializeField] public int damage;
	[SerializeField] public int type;
	[SerializeField] public Sprite icon;
	public Weapon(){
		nameOf = "defaultWeaponName";
		damage = 1000;
		type = 0;
		icon = Resources.Load<Sprite>("Sprites/DefaultSpr");
		
	}

	public void Attack(){
		if(type==0){
			//melee weapon
			
		}
	}
}
