using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    private GameObject hudInventory;
    private GameObject player;

    private GameObject[] inventorySlots;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("PlayerSprite");
        inventorySlots = GameObject.FindGameObjectsWithTag("InventoryUI");
        hudInventory = GameObject.Find("hudCanvas/hudInventory");
        foreach (GameObject slot in inventorySlots)
        {
            slot.SetActive(false);
        }
    }

    public void UpdateImage(int slotNum, Sprite icon)
    {
        //this should update the inventory image
        inventorySlots[slotNum - 1].GetComponent<UnityEngine.UI.Image>().sprite = icon;
        inventorySlots[slotNum - 1].SetActive(true);
    }
}
