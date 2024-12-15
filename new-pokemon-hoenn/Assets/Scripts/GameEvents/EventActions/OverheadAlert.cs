using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadAlert : EventAction
{
    [SerializeField] private GameObject alertObject;

    public override void DoEventAction(EventState chainState) {
        StartCoroutine(ChainGang(AlertCoroutine(), () => NextAction(chainState)));
    }

    protected IEnumerator AlertCoroutine(){
        GameObject instance = Instantiate(alertObject, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Destroy(instance);
    }
}
