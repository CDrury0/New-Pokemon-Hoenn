using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemLogic))]
public abstract class ItemEffect : MonoBehaviour
{
    protected ItemLogic itemLogic;
    public abstract IEnumerator DoItemEffect(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput);
    public abstract bool CanEffectBeUsed(Pokemon p);

    void Awake(){
        itemLogic = GetComponent<ItemLogic>();
    }
}
