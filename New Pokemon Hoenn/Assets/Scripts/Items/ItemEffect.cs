using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemLogic))]
public abstract class ItemEffect : MonoBehaviour
{
    public abstract IEnumerator DoItemEffect();
    public abstract string GetItemEffectMessage();
}
