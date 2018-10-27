using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon {

	public Fist(){
		nameOf = "Fists";
		damage = 10;
		type = 0;
		icon = Resources.Load<Sprite>("Sprites/FistSpr");
	}
}
