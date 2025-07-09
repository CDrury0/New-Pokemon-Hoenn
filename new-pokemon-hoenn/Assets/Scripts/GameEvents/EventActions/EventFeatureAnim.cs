using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Intended for use on instanced animation prefabs, not typical EventAction chains
/// </summary>
public class EventFeatureAnim : EventStateReceiver<IStateFeature>
{
    [SerializeField] SingleAnimOverride animOverride;

    protected override IEnumerator EventActionLogic() {
        // Warning: hardcoded state value jumpscare
        exit = true;
        var image = animOverride.GetComponent<Image>();
        var feature = GetSenderState()?.FirstOrDefault();
        animOverride.PlayAnimation();
        if(feature is not null){
            image.sprite = feature.GetSprite();
            yield return new WaitForSeconds(0.6f);
            AudioManager.Instance.PlaySoundEffect(feature.GetSound());
        }
    }
}
