using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadAlert : EventAction
{
    [SerializeField] private GameObject alertObject;
    protected override IEnumerator EventActionLogic(){
        GameObject instance = Instantiate(alertObject, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Destroy(instance);
    }
}
