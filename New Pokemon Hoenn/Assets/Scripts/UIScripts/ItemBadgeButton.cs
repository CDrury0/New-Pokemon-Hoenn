using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemBadgeButton : MonoBehaviour
{
    [HideInInspector] public ItemData itemData;
    [SerializeField] private Image badgeSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI quantity;
    private System.Action storedCallback;

    public void Load(ItemData itemData, int quantity, System.Action onClick = null){
        storedCallback ??= onClick;
        badgeSprite.sprite = itemData.itemSprite;
        itemName.text = itemData.itemName;
        this.itemData = itemData;
        this.quantity.text = itemData.itemPocket == ItemData.ItemPocket.Key_Items ? string.Empty : "x" + quantity;
    }

    public void ButtonClick(){
        storedCallback();
    }
}
