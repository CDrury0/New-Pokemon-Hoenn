using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    public Color grayBackgroundColor;
    public Image grayOverlay;
    public GameObject dexButton;
    public GameObject partyButton;
    public GameObject inventoryButton;
    public GameObject mapButton;
    public GameObject cardButton;
    public GameObject saveButton;
    public GameObject settingsButton;
    public GameObject gameMenu;
    public void ToggleMenu()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }


        Animator animator = gameMenu.GetComponent<Animator>();

        bool menuIsOpen = animator.GetBool("open");

        animator.SetBool("open", !menuIsOpen);

        if (menuIsOpen == false)
        {
            StartCoroutine(AntiSpam());
            LoadButtons();
        }
        else
        {
            StartCoroutine(AntiSpam());
            UnloadButtons();
        }
    }

    public void LoadButtons()
    {
        dexButton.SetActive(true);
        partyButton.SetActive(true);
        inventoryButton.SetActive(true);
        mapButton.SetActive(true);
        cardButton.SetActive(true);
        saveButton.SetActive(true);
        settingsButton.SetActive(true);
    }

    public void UnloadButtons()
    {
        dexButton.SetActive(false);
        partyButton.SetActive(false);
        inventoryButton.SetActive(false);
        mapButton.SetActive(false);
        cardButton.SetActive(false);
        saveButton.SetActive(false);
        settingsButton.SetActive(false);
    }

    private IEnumerator AntiSpam()
    {
        GlobalGameEvents.blockPlayerInputOverworld = true;
        yield return new WaitForSecondsRealtime(0.4f);
        GlobalGameEvents.blockPlayerInputOverworld = false;
    }

}
