using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalMessage : MonoBehaviour
{
    [SerializeField] private GameObject messageModalPrefab;

    public IEnumerator ShowModalMessage(string message){
        if(string.IsNullOrEmpty(message)){
            yield break;
        }
        GameObject outputInstance = Instantiate(messageModalPrefab);
        IEnumerator writeMessage = outputInstance.GetComponentInChildren<WriteText>().WriteMessageConfirm(message);
        yield return StartCoroutine(writeMessage);
        Destroy(outputInstance);
    }
}
