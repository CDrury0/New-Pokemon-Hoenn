using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainerInventoryUseMove : ButtonContainerInventoryUse
{
    [SerializeField] private GameObject moveMenuPrefab;

    public void OpenSelectMoveMenu(){
        SelectMoveMenu moveMenu = Instantiate(moveMenuPrefab).GetComponent<SelectMoveMenu>();
        Pokemon p = PlayerParty.Instance.playerParty.party[GetComponent<PartyInfoBox>().whichPartyMember];
        moveMenu.UpdateMenu(p, (int whichMove) => {
            UseButtonFunction(whichMove);
        });
    }
}
