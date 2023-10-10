using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public enum ItemPocket {Item, Berry, Key, Ball, Machine}
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemPocket itemPocket;
    public bool canBeHeld;
    public int itemPrice;
    //public bool usedDuringBattle;
    //public bool usedOutsideBattle;
    //public PokemonDefault[] worksOn;
}
