using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    private Sprite slot1 = null;
    private Sprite slot2 = null;
    private Sprite slot3 = null;
    private Sprite slot4 = null;
    private Sprite slot5 = null;

    private GameObject playerInventory;
    private GameObject player;
    // Use this for initialization
    void Start()
    {
        this.playerInventory = GameObject.Find("Inventory");
        this.player = GameObject.FindWithTag("Player");
        Debug.Log("INVENTORY");
    }

    void updateImage(int slotNum, GameObject weapon)
    {
        //this should update the inventory image
    }
}
