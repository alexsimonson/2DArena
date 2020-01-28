using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{

    public Weapon spawnedWeapon;
    public BasicPistol basicPistol;
    public BasicKnife basicKnife;
    public BasicKnife advancedKnife;
    public BasicPistol advancedPistol;
    public BasicKnife shitAxe;

    [SerializeField] public bool spawnKnife;
    [SerializeField] public bool spawnGun;
    public AudioClip pickupAudio;

    //could I then do weapon = new Pistol() or something like that?  to set the base stats of the gun
    BoxCollider2D bc;

    // Use this for initialization
    void Start()
    {
        float randomRange = Random.Range(0.0f, 1.0f);

        //constructor for new items (nameOf, damage, attackSpeed, spriteName)
        basicPistol = new BasicPistol();
        basicKnife = new BasicKnife();
        advancedKnife = new BasicKnife("Advanced Dagger", 100, .6f, "dagger 2");
        shitAxe = new BasicKnife("Shitty Axe", 35, .3f, "ShitAxe");
        advancedPistol = new BasicPistol("Advanced Pistol", 200, .1f, 1000f, 20, "gun 2");
        pickupAudio = Resources.Load<AudioClip>("Audio/Basic Reload");

        if (spawnKnife && spawnGun)
        {
            if (randomRange < 0.5f)
            {
                spawnedWeapon = basicPistol;
            }
            else if (randomRange >= .5f)
            {
                spawnedWeapon = advancedKnife;
            }
            //this will rotate guns appropriately every time
            if (spawnedWeapon.type == 1)
            {
                gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
            }
        }
        else if (spawnKnife)
        {
            spawnedWeapon = advancedKnife;
        }
        else if (spawnGun)
        {
            spawnedWeapon = advancedPistol;
        }
        else
        {
            if (randomRange < 0.25f)
            {
                spawnedWeapon = basicPistol;
            }
            else if (randomRange >= 0.25f && randomRange < .5f)
            {
                spawnedWeapon = advancedKnife;
            }
            else if (randomRange >= 0.5f && randomRange < .75f)
            {
                spawnedWeapon = advancedPistol;
            }
            else if (randomRange >= .75f && randomRange < 1f)
            {
                spawnedWeapon = basicKnife;
            }
            //this will rotate guns appropriately every time
        }
        // flip sprite if gun because of the way they are
        if (spawnedWeapon.type == 1)
        {
            gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
        }
        //this will ensure the sprite is changed every time
        gameObject.GetComponent<SpriteRenderer>().sprite = spawnedWeapon.icon;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Weapons";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log(col.gameObject.name + " could pick up the " + spawnedWeapon.nameOf);
    }

}
