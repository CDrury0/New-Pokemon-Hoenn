using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private GameObject pocketPanelPrefab;
    [SerializeField] private GameObject pocketHeaderPrefab;
    private GameObject pocketPanel;
    private GameObject pocketHeader;
    [SerializeField] private GameObject itemBadgePrefab;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject giveButton;
    [SerializeField] private GameObject sellButton;
    [SerializeField] private ModalMessage modal;
    private const string DEFAULT_DESCRIPTION = "Select an item to learn more about it.";
    private List<ItemBadgeButton> itemBadgeButtons;
    public static ItemLogic LoadedItemInstance { get; private set; }

    void OnEnable(){
        itemBadgeButtons = new List<ItemBadgeButton>();
        BuildItemPanel();
    }

    private void BuildItemPanel(){
        //there will be one pocket for each value in the enum
        int numPockets = System.Enum.GetValues(typeof(ItemData.ItemPocket)).Length;
        List<KeyValuePair<ItemData, int>> kvpList = PlayerInventory.GetKeyValuePairList();
        for (int i = 0; i < numPockets; i++){
            List<KeyValuePair<ItemData, int>> kvpPocket = kvpList.FindAll(entry => entry.Key.itemPocket == (ItemData.ItemPocket)i);
            if(kvpPocket.Count > 0){
                kvpList.RemoveAll(entry => entry.Key.itemPocket == (ItemData.ItemPocket)i);
                pocketHeader = Instantiate(pocketHeaderPrefab, Vector3.zero, Quaternion.identity, scrollContent);
                pocketHeader.GetComponent<TextMeshProUGUI>().text = ((ItemData.ItemPocket)i).ToString().Replace('_', ' ');
                pocketPanel = Instantiate(pocketPanelPrefab, Vector3.zero, Quaternion.identity, scrollContent);
            }
            foreach (KeyValuePair<ItemData, int> kvp in kvpPocket){
                ItemBadgeButton itemBadge = Instantiate(itemBadgePrefab, Vector3.zero, Quaternion.identity, pocketPanel.transform).GetComponent<ItemBadgeButton>();
                itemBadge.Load(kvp.Key, kvp.Value, () => { LoadItem(kvp.Key); });
                itemBadgeButtons.Add(itemBadge);
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollContent);
    }

    void OnDisable(){
        if(LoadedItemInstance != null){
            Destroy(LoadedItemInstance.gameObject);
        }
        LoadedItemInstance = null;
        Destroy(transform.parent.gameObject);
    }

    private void SetDescriptionPanel(string description = DEFAULT_DESCRIPTION, bool allowUse = false, bool allowGive = false){
        itemDescription.text = description;
        useButton.SetActive(allowUse);
        giveButton.SetActive(allowGive);
    }

    public void UpdateBadge(ItemData itemData){
        ItemBadgeButton b = itemBadgeButtons.Find(i => i.itemData == itemData);
        int numAvailable = PlayerInventory.GetInventoryQuantity(itemData);
        SetDescriptionPanel();
        if(numAvailable == 0){
            if(b.transform.parent.childCount == 1){
                //destroys the pocket heading and panel gameobjects that correspond to the pocket this badge belongs to
                Destroy(b.transform.parent.parent.GetChild(b.transform.parent.GetSiblingIndex() - 1).gameObject);
                Destroy(b.transform.parent.gameObject);
            }
            else{
                Destroy(b.gameObject);
            }
            itemBadgeButtons.Remove(b);
        }
        else{
            b.Load(itemData, numAvailable);
        }
    }

    public void LoadItem(ItemData itemData){
        if(LoadedItemInstance != null){
            Destroy(LoadedItemInstance.gameObject);
        }
        LoadedItemInstance = itemData.itemLogicGO != null ? Instantiate(itemData.itemLogicGO).GetComponent<ItemLogic>() : null;
        bool allowUse = LoadedItemInstance != null && LoadedItemInstance.GetAllowUse();
        bool allowGive = LoadedItemInstance != null && LoadedItemInstance.heldItem != null && !CombatSystem.BattleActive;
        SetDescriptionPanel(itemData.itemDescription, allowUse, allowGive);
    }

    public void UseItemButtonFunction(){
        if(LoadedItemInstance.itemData.usedWithoutTarget){
            PlayerInventory.SubtractItem(LoadedItemInstance.itemData);
            StartCoroutine(LoadedItemInstance.DoItemEffects(null, null, (string message) => modal.ShowModalMessage(message)));
            UpdateBadge(LoadedItemInstance.itemData);
            return;
        }
        useButton.GetComponent<OverlayTransitionCaller>().CallTransition();
    }
}
