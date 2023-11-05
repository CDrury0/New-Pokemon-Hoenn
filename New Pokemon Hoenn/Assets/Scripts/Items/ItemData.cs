using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public GameObject itemLogicGO;
    public enum ItemPocket {Items, Berries, Key_Items, Balls, Machines}
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemPocket itemPocket;
    public int itemPrice;
    public bool usedWithoutTarget;
    public int stackLimit;
}
