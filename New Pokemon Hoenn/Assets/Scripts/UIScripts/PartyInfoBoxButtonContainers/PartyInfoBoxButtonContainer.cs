using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartyInfoBoxButtonContainer : MonoBehaviour
{
    [SerializeField] private GameObject messageModalPrefab;
    protected PartyMenu partyMenu;
    public abstract void LoadActionButtons(Pokemon p);
    protected void DisableAlternateInputs(){
        partyMenu.closeButton.SetActive(false);
        foreach(PartyInfoBox box in partyMenu.infoBoxes){
            box.actionButtonPanel.SetActive(false);
        }
    }

    public IEnumerator ShowModalMessage(string message){
        GameObject outputInstance = Instantiate(messageModalPrefab);
        IEnumerator writeMessage = outputInstance.GetComponentInChildren<WriteText>().WriteMessageConfirm(message);
        yield return StartCoroutine(writeMessage);
        Destroy(outputInstance);
    }

    void Awake(){
        partyMenu = GetComponentInParent<PartyMenu>();
    }
}
