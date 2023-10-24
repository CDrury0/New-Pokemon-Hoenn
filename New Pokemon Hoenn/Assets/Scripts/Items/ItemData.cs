using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public GameObject itemLogicGO;
    public enum ItemPocket {Item, Berry, Key, Ball, Machine}
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemPocket itemPocket;
    public int itemPrice;
    public bool usedWithoutTarget;
}
