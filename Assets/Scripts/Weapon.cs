using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour{
	[SerializeField] public string nameOf;
	[SerializeField] public int damage;
	[SerializeField] public int type;

	public Weapon(){
		nameOf = "defaultWeaponName";
		damage = 1000;
		type = 0;
	}
}
