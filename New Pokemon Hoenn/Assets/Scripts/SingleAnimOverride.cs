using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimOverride : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip clip;
    [SerializeField] private bool playOnAwake = true;
    public float ClipLength { get { return clip.length; } }

    void Awake() {
        AnimatorOverrideController overrider = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrider["FadeIn"] = clip;
        animator.runtimeAnimatorController = overrider;
        if(playOnAwake){
            PlayAnimation();
        }
    }

    public void PlayAnimation(){
        animator.SetBool("PlayClip", true);
        StartCoroutine(SetBoolDelayed());
    }

    /// <summary>
    /// Yields an amount of time equal to the length of the clip
    /// </summary>
    public IEnumerator PlayAnimationWait(){
        PlayAnimation();
        yield return new WaitForSeconds(ClipLength);
    }

    private IEnumerator SetBoolDelayed(){
        yield return null;
        animator.SetBool("PlayClip", false);
    }
}
