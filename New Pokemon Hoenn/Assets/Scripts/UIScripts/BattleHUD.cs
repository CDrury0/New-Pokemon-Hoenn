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
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public ColorSO typeColors;
    public Sprite[] genderSprites;

    public void SetBattleHUD(Pokemon p){
        healthBar.SetHealthBarInstant(p.CurrentHealth, p.stats[0]);
        expBar.SetExpBarInstant(p.experience, StatLib.CalculateExperienceAtLevel(p.pokemonDefault.growthRate, p.level), StatLib.CalculateExperienceAtLevel(p.pokemonDefault.growthRate, p.level + 1));
        SetStatusIcon(p.primaryStatus);
        SetGenderIcon(p.gender);
        nameText.text = p.nickName;
        levelText.text = "Lv. " + p.level;
    }

    private void SetStatusIcon(PrimaryStatus status){
        if(status == PrimaryStatus.None){
            statusIcon.gameObject.SetActive(false);
            return;
        }
        
        statusIcon.color = typeColors.colors[GetStatusTypeNumber(status)];
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

    private int GetStatusTypeNumber(PrimaryStatus status){
        switch(status){
            case PrimaryStatus.Poisoned:
            return (int)StatLib.Type.Poison;
            case PrimaryStatus.Burned:
            return (int)StatLib.Type.Fire;
            case PrimaryStatus.Paralyzed:
            return (int)StatLib.Type.Electric;
            case PrimaryStatus.Asleep:
            return (int)StatLib.Type.Normal;
            case PrimaryStatus.Frozen:
            return (int)StatLib.Type.Ice;
            default:
            return (int)StatLib.Type.None;
        }
    }
}
