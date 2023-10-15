using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemBadgeButton : MonoBehaviour
{
    [SerializeField] private Image badgeSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI quantity;
    private System.Action sideEffect;

    public void Load(ItemData itemData, int quantity, System.Action onClick){
        sideEffect = onClick;
        badgeSprite.sprite = itemData.itemSprite;
        itemName.text = itemData.itemName;
        this.quantity.text = itemData.itemPocket == ItemData.ItemPocket.Key ? string.Empty : "x" + quantity;
    }

    public void ButtonClick(){
        sideEffect();
    }
}
