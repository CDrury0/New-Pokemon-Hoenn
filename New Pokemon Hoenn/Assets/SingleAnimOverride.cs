using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimOverride : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip clip;

    void Awake() {
        AnimatorOverrideController overrider = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrider["FadeIn"] = clip;
        animator.runtimeAnimatorController = overrider;
    }
}
