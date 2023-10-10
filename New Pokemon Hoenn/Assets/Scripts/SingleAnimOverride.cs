using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimOverride : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip[] clips;
    [Tooltip("Clip 0 will play on awake")] [SerializeField] private bool playOnAwake = true;
    private AnimationClip currentClip;
    private RuntimeAnimatorController baseController;
    public float ClipLength { get { return currentClip.length; } }

    void Awake() {
        baseController = animator.runtimeAnimatorController;
        if(playOnAwake){
            PlayAnimation(0);
        }
    }

    public void PlayAnimation(int clipIndex = 0){
        UpdateAnimation(clips[clipIndex]);
        animator.SetBool("PlayClip", true);
        StartCoroutine(SetBoolDelayed());
    }

    public void PlayAnimation(AnimationClip clip){
        UpdateAnimation(clip);
        animator.SetBool("PlayClip", true);
        StartCoroutine(SetBoolDelayed());
    }

    /// <summary>
    /// Yields an amount of time equal to the length of the clip
    /// </summary>
    public IEnumerator PlayAnimationWait(int clipIndex = 0){
        PlayAnimation(clipIndex);
        yield return new WaitForSeconds(ClipLength);
    }

    /// <summary>
    /// Yields an amount of time equal to the length of the clip
    /// </summary>
    public IEnumerator PlayAnimationWait(AnimationClip clip){
        PlayAnimation(clip);
        yield return new WaitForSeconds(ClipLength);
    }

    private void UpdateAnimation(AnimationClip clip){
        if(clip != currentClip){
            currentClip = clip;
            AnimatorOverrideController overrider = new AnimatorOverrideController(baseController);
            overrider["FadeIn"] = clip;
            animator.runtimeAnimatorController = overrider;
        }
    }

    private IEnumerator SetBoolDelayed(){
        yield return null;
        animator.SetBool("PlayClip", false);
    }
}
