using UnityEngine;
using System.Collections;

public class Weapon
{
    [SerializeField] public string nameOf;
    [SerializeField] public int damage;
    [SerializeField] public int type;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public string ammoType;
    [SerializeField] public int magazineSize;
    [SerializeField] public int ammoLoaded;
    [SerializeField] public int ammoPool;
    [SerializeField] public int addAmmo;
    [SerializeField] public Sprite icon;
    [SerializeField] public Sprite bullet;
    public Weapon()
    {
        nameOf = "defaultWeaponName";
        damage = 1000;
        type = 0;
        attackSpeed = .5f;
        bulletSpeed = 1000;
        ammoType = ".50 Caliber";
        magazineSize = 10;
        ammoLoaded = magazineSize;
        ammoPool = 30;
        addAmmo = 30;
        // icon = Resources.Load<Sprite>("Sprites/DefaultSpr");
        bullet = Resources.Load<Sprite>("Sprites/BulletSpr");
    }

    public void Attack()
    {
        if (type == 0)
        {
            //melee weapon

        }
    }
}
