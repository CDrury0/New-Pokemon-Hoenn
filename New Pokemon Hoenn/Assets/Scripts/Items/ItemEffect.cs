using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemLogic))]
public abstract class ItemEffect : MonoBehaviour
{
    public abstract IEnumerator DoItemEffect();
    public abstract bool CanEffectBeUsed(Pokemon p);
    
    //is this really the best way to get a message to output after an item is used?
    public abstract string GetItemEffectMessage();
}
