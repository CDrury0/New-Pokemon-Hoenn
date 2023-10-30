using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalMessage : MonoBehaviour
{
    [SerializeField] private GameObject messageModalPrefab;

    public IEnumerator ShowModalMessage(string message){
        GameObject outputInstance = GameObject.Instantiate(messageModalPrefab);
        IEnumerator writeMessage = outputInstance.GetComponentInChildren<WriteText>().WriteMessageConfirm(message);
        yield return StartCoroutine(writeMessage);
        GameObject.Destroy(outputInstance);
    }
}
