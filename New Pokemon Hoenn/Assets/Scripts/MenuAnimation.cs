using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject menuButtonLayout;

    public void ToggleMenu() {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        bool menuIsOpen = animator.GetBool("open");
        animator.SetBool("open", !menuIsOpen);
        menuButtonLayout.SetActive(!menuIsOpen);
    }
}
