using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : EventAction
{
    [SerializeField] Vector3 warpTo;
    [SerializeField] AreaData newArea;

    protected override IEnumerator EventActionLogic() {
        var loaded = false;
        yield return StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(() => {
            var areaToLoad = newArea?.GetGameAreaManager() ?? GetComponentInParent<GameAreaManager>();
            IEnumerator WaitLoadArea() {
                yield return StartCoroutine(areaToLoad?.LoadArea());
                loaded = true;
            }
            StartCoroutine(WaitLoadArea());
            PlayerInput.SetPlayerPosition(warpTo);
        }));
        yield return new WaitUntil(() => loaded);
    }
}
