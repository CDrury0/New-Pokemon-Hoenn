using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGiveItem : EventAction
{
    [SerializeField] private ItemData item;
    [SerializeField] private int quantity = 1;
    [SerializeField] private GameObject textObj;

    protected override IEnumerator EventActionLogic(EventState state){
        PlayerInventory.AddItem(item, quantity);
        string qtyMsg = quantity == 1 ? string.Empty : quantity + "x ";
        string message = "Player received " + qtyMsg + item.itemName;

        GameObject textObjInstance = Instantiate(textObj);
        yield return StartCoroutine(textObjInstance.GetComponentInChildren<WriteText>().WriteMessageConfirm(message, 2f));
        Destroy(textObjInstance);
    }
}
