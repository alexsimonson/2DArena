using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] inventorySlots;


    // Use this for initialization
    void Start()
    {
        inventorySlots = GameObject.FindGameObjectsWithTag("InventoryUI");
        HideInventory();
    }

    public void UpdateImage(int slotNum, Sprite icon)
    {
        //this should update the inventory image
        inventorySlots[slotNum - 1].GetComponent<UnityEngine.UI.Image>().sprite = icon;
        inventorySlots[slotNum - 1].SetActive(true);
    }

    public void HideInventory()
    {
        foreach (GameObject slot in inventorySlots)
        {
            slot.SetActive(false);
        }
    }
}
