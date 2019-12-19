using UnityEngine;
using System.Collections;

public class BasicPistol : Weapon
{

    public BasicPistol()
    {
        nameOf = "Basic Pistol";
        damage = 1000;
        type = 1;
        attackSpeed = .1f;
        ammoType = ".50 Caliber";
        magazineSize = 7;
        ammoLoaded = magazineSize;
        ammoPool = magazineSize * 3;
        addAmmo = magazineSize * 4;
        icon = Resources.Load<Sprite>("Sprites/ShitGunSpr");
        bullet = Resources.Load<Sprite>("Sprites/BulletSpr");
    }

    public BasicPistol(string newName, int newDamage, float newAttackSpeed, int newMagazineSize, string iconName)
    {
        nameOf = newName;
        damage = newDamage;
        attackSpeed = newAttackSpeed;
        magazineSize = newMagazineSize;
        ammoLoaded = newMagazineSize;
        ammoPool = newMagazineSize * 3;
        addAmmo = newMagazineSize * 4;
        icon = Resources.Load<Sprite>("Sprites/" + iconName);
        bullet = null;
    }
}