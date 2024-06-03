using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaMarkerHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textChild;
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip inClip;

    void Start() {
        textChild.text = ReferenceLib.ActiveArea.areaName;
        StartCoroutine(HandleAnim());
    }

    private IEnumerator HandleAnim() {
        yield return new WaitForSeconds(inClip.length + 2.5f);
        anim.SetBool("DoOut", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
