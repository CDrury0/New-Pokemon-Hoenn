using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public ItemData itemData;
    [Tooltip("If null, the item cannot be held")]
    public HeldItem heldItem;
    [Tooltip("If length is 0, the item cannot be used during battle")]
    public List<ItemEffect> onUseDuringBattle;
    [Tooltip("If length is 0, the item cannot be used outside battle")]
    public List<ItemEffect> onUseOutsideBattle;
    //public PokemonDefault[] worksOn;
}
