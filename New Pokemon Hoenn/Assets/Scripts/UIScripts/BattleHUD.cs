using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public HealthBar healthBar;
    public ExpBar expBar;
    public Image statusIcon;
    public Image genderIcon;
    public GameObject caughtIcon;
    [SerializeField] private SingleAnimOverride animOverride;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public ColorSO statusColors;
    public Sprite[] genderSprites;

    public void SetBattleHUD(Pokemon p){
        healthBar.SetHealthBarInstant(p.CurrentHealth, p.stats[0]);
        expBar.SetExpBarInstant(p.experience, p.pokemonDefault.CalculateExperienceAtLevel(p.level), p.pokemonDefault.CalculateExperienceAtLevel(p.level + 1));
        SetStatusIcon(p.primaryStatus);
        SetGenderIcon(p.gender);
        SetCaughtIcon(p);
        nameText.text = p.nickName;
        levelText.text = "Lv. " + p.level;
    }

    public void SlideIn(){
        gameObject.SetActive(true);
        animOverride.PlayAnimation(0);
    }

    public void SlideOut(){
        if(gameObject.activeInHierarchy){
            StartCoroutine(SlideOutCoroutine());
        }
    }

    private IEnumerator SlideOutCoroutine(){
        yield return StartCoroutine(animOverride.PlayAnimationWait(1));
        gameObject.SetActive(false);
    }

    private void SetCaughtIcon(Pokemon p){
        if(!CombatSystem.BattleActive || ReferenceLib.GlobalDexProgress[p.pokemonDefault.IDNumber] != DexStatus.Caught){
            caughtIcon.SetActive(false);
            return;
        }
        // If p is not on the enemy team, don't show the icon        
        caughtIcon.SetActive(CombatSystem.BattleTargets.Find(b => b.pokemon == p && !b.teamBattleModifier.isPlayerTeam) != null);
    }

    private void SetStatusIcon(PrimaryStatus status){
        if(status == PrimaryStatus.None){
            statusIcon.gameObject.SetActive(false);
            return;
        } 
        statusIcon.color = statusColors.colors[(int)status];
        statusIcon.GetComponentInChildren<TextMeshProUGUI>(true).text = status.ToString();
        statusIcon.gameObject.SetActive(true);
    }

    private void SetGenderIcon(Gender gender){
        if(gender == Gender.None){
            genderIcon.enabled = false;
            return;
        }
        genderIcon.sprite = genderSprites[(int)gender - 1];
        genderIcon.enabled = true;
    }
}
