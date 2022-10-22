using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldItem : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public GameObject testMenu;
    public Item thisItem;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //EnterInventory(thisItem);
        testMenu.SetActive(!testMenu.activeInHierarchy);
    }

    public void EnterInventory(Item thisItem)
    {
        GlobalGameEvents.globalPlayerInventory.Add(thisItem);
        Debug.Log("picked up " + thisItem.itemName);
        Destroy(this.gameObject);
    }
}
