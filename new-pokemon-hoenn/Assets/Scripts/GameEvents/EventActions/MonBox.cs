using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonBox : EventAction {
    public GameObject menuPrefab;
    protected override IEnumerator EventActionLogic() {
        bool proceed = false;
        StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(() => {
            var parentObj = Instantiate(menuPrefab);
            var closeBtn = GameObject.FindGameObjectWithTag("BoxClose");
            closeBtn.GetComponent<OverlayTransitionCaller>()?.action.AddListener(() => {
                proceed = true;
                Destroy(parentObj);
            });
        }));
        yield return new WaitUntil(() => proceed);
    }
}
