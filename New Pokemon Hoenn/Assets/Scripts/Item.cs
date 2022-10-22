using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Item, Berry, Key, Ball, Machine}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int itemIdentifier;
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemType itemPocket;
    public bool canBeHeld;
    public bool usedDuringBattle;
    public bool usedOutsideBattle;
    public int itemPrice;
    public PokemonDefault[] worksOn;
}
