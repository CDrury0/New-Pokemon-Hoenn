using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DexStatus { Unknown, Seen, Caught}
public class LoadDexInfo : MonoBehaviour
{
    const int CHART_MAX_STAT = 200;
    public TextMeshProUGUI dexButtonName;
    public TextMeshProUGUI dexNumber;
    public GameObject caughtSprite;
    public Image dexSprite;
    public LoadDexUI loadDexUI;
    public PokemonDefault representsThisPokemon;
    public int alphaOrder;
    private DexStatus caughtStatus;

    private void OnEnable() {
        caughtStatus = ReferenceLib.GlobalDexProgress[representsThisPokemon.IDNumber];
        caughtSprite.SetActive(caughtStatus == DexStatus.Caught);
        dexNumber.text = representsThisPokemon.IDNumber.ToString();
        dexButtonName.text = caughtStatus == DexStatus.Unknown ? "???" : representsThisPokemon.pokemonName;
    }

    public void LoadDexDetails() {
        bool isCaught = caughtStatus == DexStatus.Caught;
        loadDexUI.type1box.SetActive(false);
        loadDexUI.type2box.SetActive(false);
        if(isCaught){
            LoadPokemonTypes(representsThisPokemon);
        }

        loadDexUI.heightBox.text = isCaught ? "Avg. Height: " + representsThisPokemon.height + " m" : string.Empty;
        loadDexUI.weightBox.text = isCaught ? "Avg. Weight: " + representsThisPokemon.weight + " kg" : string.Empty;
        loadDexUI.speciesBox.text = isCaught ? representsThisPokemon.species + " Pokémon" : string.Empty;
        dexSprite.sprite = isCaught ? representsThisPokemon.normalFront : loadDexUI.mysterySprite;

        loadDexUI.dexEntry.text = caughtStatus switch {
            DexStatus.Caught => representsThisPokemon.pokedexEntry,
            DexStatus.Seen => "Catch this Pokémon to learn more about it.",
            DexStatus.Unknown => "Encounter this Pokémon to learn more about it.",
            _ => "ERROR"
        };

        LoadLocationText();
        LoadSpriteNameText(caughtStatus != DexStatus.Unknown);
        LoadEggGroups(isCaught);
        LoadEVYield(isCaught);
        LoadStatBars(isCaught, representsThisPokemon);
    }

    public void LoadEVYield(bool load) {
        if(!load){
            loadDexUI.evYieldText.text = "Unknown";
            return;
        }
        
        loadDexUI.evYieldText.text = string.Empty;
        for(int i = 0; i < representsThisPokemon.evYield.Length; i++){
            if(representsThisPokemon.evYield[i] != 0){

                if(loadDexUI.evYieldText.text != string.Empty){
                    loadDexUI.evYieldText.text += ", ";
                }

                // The amount of EVs at the index (stat) specified as a string
                string evStat = representsThisPokemon.evYield[i].ToString();
                evStat += " " + i switch {
                    0 => "HP",
                    1 => "Attack",
                    2 => "Defense",
                    3 => "Special Attack",
                    4 => "Special Defense",
                    5 => "Speed",
                    _ => "ERROR"
                };

                loadDexUI.evYieldText.text += evStat;
            }
        }
    }

    public void LoadSpriteNameText(bool load) {
        loadDexUI.spriteNameText.text = load ? representsThisPokemon.pokemonName : "???";
    }

    public void LoadEggGroups(bool load) {
        if(!load){
            loadDexUI.eggGroupText.text = "Undiscovered";
            return;
        }

        loadDexUI.eggGroupText.text = representsThisPokemon.eggGroup1.ToString();
        if(representsThisPokemon.eggGroup2 != StatLib.EggGroup.None){
            loadDexUI.eggGroupText.text += ", ";
            loadDexUI.eggGroupText.text += representsThisPokemon.eggGroup2.ToString();
        }
    }

    public void LoadLocationText() {
        loadDexUI.locationText.text = representsThisPokemon.lastSeen?.areaName ?? "Area Unknown";
    }

    public void ResetStatBars() {
        foreach(Image image in loadDexUI.statBars){
            image.fillAmount = 0;
        }
    }

    public void LoadStatBars(bool load, PokemonDefault representsThis) {
        if(!load){
            ResetStatBars();
            return;
        }

        for(int i = 0; i < loadDexUI.statBars.Length; i++){
            loadDexUI.statBars[i].fillAmount = (float)representsThis.baseStats[i] / CHART_MAX_STAT;

            loadDexUI.statBars[i].color = representsThis.baseStats[i] switch {
                >= 150 => loadDexUI.statColors[5],
                >= 115 => loadDexUI.statColors[4],
                >= 85 => loadDexUI.statColors[3],
                >= 55 => loadDexUI.statColors[2],
                >= 30 => loadDexUI.statColors[1],
                _ => loadDexUI.statColors[0]
            };
        }
    }

    public void LoadPokemonTypes(PokemonDefault representsThis){
        loadDexUI.type1box.gameObject.SetActive(true);

        loadDexUI.type1box.GetComponentInChildren<Image>().color = representsThis.type1.typeColor;
        loadDexUI.type1box.GetComponentInChildren<TextMeshProUGUI>().text = representsThis.type1.name;

        bool hasSecond = representsThis.type2 != null;
        if(hasSecond){
            loadDexUI.type2box.GetComponentInChildren<Image>().color = representsThis.type2.typeColor;
            loadDexUI.type2box.GetComponentInChildren<TextMeshProUGUI>().text = representsThis.type2.name;
        }
        loadDexUI.type2box.gameObject.SetActive(hasSecond);
    }
}
