using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DexStatus { Unknown, Seen, Caught}
public class LoadDexInfo : MonoBehaviour
{
    public static DexStatus[] globalDexProgress = new DexStatus[215];
    public Image type1fill;
    public Image type2fill;
    public Text type1text;
    public Text type2text;
    public GameObject type1box;
    public GameObject type2box;
    public Text dexButtonName;
    public Text dexNumber;
    public GameObject caughtSprite;
    public Text heightBox;
    public Text weightBox;
    public Text speciesBox;
    public Text dexEntry;
    public Image dexSprite;
    public LoadDexUI loadDexUI;
    public ReferenceLib pokemonReferenceLib;
    public PokemonDefault representsThisPokemon;
    private DexStatus caughtStatus;

    private void OnEnable() {
        caughtStatus = globalDexProgress[representsThisPokemon.IDNumber];
        caughtSprite.SetActive(caughtStatus == DexStatus.Caught);
        dexNumber.text = representsThisPokemon.IDNumber.ToString();
        dexButtonName.text = caughtStatus == DexStatus.Unknown ? "???" : representsThisPokemon.pokemonName;
    }

    public void LoadDexDetails() {
        type1box.SetActive(false);
        type2box.SetActive(false);

        if (caughtStatus == DexStatus.Caught){
            heightBox.text = "Avg. Height: " + representsThisPokemon.height + " m";
            weightBox.text = "Avg. Weight: " + representsThisPokemon.weight + " kg";
            speciesBox.text = representsThisPokemon.species + " Pokémon";
            dexEntry.text = representsThisPokemon.pokedexEntry;
            dexSprite.sprite = representsThisPokemon.normalFront;
            LoadPokemonTypes(representsThisPokemon);
            LoadLocationText();
            LoadSpriteNameText(true);
            LoadEggGroups(true);
            LoadEVYield(true);
            LoadStatBars(representsThisPokemon);
        }
        else if(caughtStatus == DexStatus.Seen){
            heightBox.text = "";
            weightBox.text = "";
            speciesBox.text = "";
            dexEntry.text = "Catch this Pokémon to learn more about it.";
            dexSprite.sprite = representsThisPokemon.normalFront;
            LoadLocationText();
            LoadSpriteNameText(true);
            LoadEggGroups(false);
            LoadEVYield(false);
            ResetStatBars();
        }
        else if(caughtStatus == DexStatus.Unknown){
            heightBox.text = "";
            weightBox.text = "";
            speciesBox.text = "";
            dexEntry.text = "Encounter this Pokémon to learn more about it.";
            dexSprite.sprite = loadDexUI.mysterySprite;
            LoadLocationText();
            LoadSpriteNameText(false);
            LoadEggGroups(false);
            LoadEVYield(false);
            ResetStatBars();
        }
    }

    public void LoadEVYield(bool load)
    {
        if(load == true)
        {
            loadDexUI.evYieldText.text = "";

            for(int i = 0; i < representsThisPokemon.evYield.Length; i++)
            {
                if(representsThisPokemon.evYield[i] != 0)
                {
                    string evStat = representsThisPokemon.evYield[i].ToString();

                    switch (i)
                    {
                        case 0:
                            evStat += " HP";
                            break;
                        case 1:
                            evStat += " Attack";
                            break;
                        case 2:
                            evStat += " Defense";
                            break;
                        case 3:
                            evStat += " Special Attack";
                            break;
                        case 4:
                            evStat += " Special Defense";
                            break;
                        case 5:
                            evStat += " Speed";
                            break;
                    }

                    if(loadDexUI.evYieldText.text != "")
                    {
                        loadDexUI.evYieldText.text += ", ";
                    }

                    loadDexUI.evYieldText.text += evStat;
                }
            }
        }
        else
        {
            loadDexUI.evYieldText.text = "Unknown";
        }
    }

    public void LoadSpriteNameText(bool load)
    {
        if(load == true)
        {
            loadDexUI.spriteNameText.text = representsThisPokemon.pokemonName;
        }
        else
        {
            loadDexUI.spriteNameText.text = "???";
        }
    }

    public void LoadEggGroups(bool load)
    {
        if(load == true)
        {
            loadDexUI.eggGroupText.text = representsThisPokemon.eggGroup1.ToString();

            if(representsThisPokemon.eggGroup2 != StatLib.EggGroup.None)
            {
                loadDexUI.eggGroupText.text += ", ";
                loadDexUI.eggGroupText.text += representsThisPokemon.eggGroup2.ToString();
            }
        }
        else
        {
            loadDexUI.eggGroupText.text = "Undiscovered";
        }
    }

    public void LoadLocationText()
    {
        loadDexUI.locationText.text = representsThisPokemon.lastSeen == null ? "Area Unknown" : representsThisPokemon.lastSeen.areaName;
    }

    public void ResetStatBars()
    {
        foreach(Image image in loadDexUI.statBars)
        {
            image.fillAmount = 0;
        }
    }

    public void LoadStatBars(PokemonDefault representsThis)
    {
        for(int i = 0; i < loadDexUI.statBars.Length; i++)
        {
            loadDexUI.statBars[i].fillAmount = (float)(representsThis.baseStats[i] / 200f);
            
            if(representsThis.baseStats[i] >= 150)
            {
                loadDexUI.statBars[i].color = loadDexUI.statColors[5];
            }
            else if(representsThis.baseStats[i] >= 115)
            {
                loadDexUI.statBars[i].color = loadDexUI.statColors[4];
            }
            else if(representsThis.baseStats[i] >= 85)
            {
                loadDexUI.statBars[i].color = loadDexUI.statColors[3];
            }
            else if(representsThis.baseStats[i] >= 55)
            {
                loadDexUI.statBars[i].color = loadDexUI.statColors[2];
            }
            else if(representsThis.baseStats[i] >= 30)
            {
                loadDexUI.statBars[i].color = loadDexUI.statColors[1];
            }
            else
            {
                loadDexUI.statBars[i].color = loadDexUI.statColors[0];
            }
        }
    }

    public void LoadPokemonTypes(PokemonDefault representsThis)
    {
        type1box.gameObject.SetActive(true);
        type1fill.color = loadDexUI.typeColors.colors[(int)representsThis.type1];
        type1text.text = representsThis.type1.ToString();

        if (representsThis.type2 == Pokemon.Type.None)
        {
            type2box.gameObject.SetActive(false);
        }
        else
        {
            type2box.gameObject.SetActive(true);
            type2fill.color = loadDexUI.typeColors.colors[(int)representsThis.type2];
            type2text.text = representsThis.type2.ToString();
        }
    }
}
