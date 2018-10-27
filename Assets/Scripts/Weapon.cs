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
		icon = Resources.Load<Sprite>("Sprites/BasicGunSpr");
		//Load a text file (Assets/Resources/Text/textFile01.txt)
        var textFile = Resources.Load<TextAsset>("Text/textFile01");
	}
}
