using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMoveMenu : MonoBehaviour
{
    [SerializeField] private SummaryMovePlate[] buttons;

    public void UpdateMenu(Pokemon p, System.Action<int> action){
        for(int i = 0; i < buttons.Length; i++){
            MoveData data = p.moves[i]?.GetComponent<MoveData>();
            if(data == null){
                buttons[i].gameObject.SetActive(false);
                continue;
            }
            ICheckMoveItemUse check = InventoryMenu.LoadedItemInstance.GetComponent<ICheckMoveItemUse>();
            if(p.moves[i] == null || !check.CanBeUsed(p, i)){
                buttons[i].button.interactable = false;
            }
            buttons[i].SetMoveInfo(p.movePP[i], p.moveMaxPP[i], data, p);
            int avoidStupidClosureRule = i;
            buttons[i].button.onClick.AddListener(() => {
                action(avoidStupidClosureRule);
                DestroySelf();
            });
        }
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}
