using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDexUI : MonoBehaviour
{
    public ColorSO typeColors;
    public Color[] statColors = new Color[6];
    public Image[] statBars = new Image[6];
    public GameObject type1box;
    public GameObject type2box;
    public Text heightBox;
    public Text weightBox;
    public Text speciesBox;
    public Text dexEntry;
    public Text locationText;
    public Text spriteNameText;
    public Text eggGroupText;
    public Text evYieldText;
    public Text seenText;
    public Text caughtText;
    public Image dexSprite;
    public Sprite mysterySprite;
    public ReferenceLib pokemonReferenceLib;
    public LoadDexInfo[] dexPlates;

    void OnDisable(){
        Destroy(transform.parent.gameObject);
    }

    private void OnEnable() {
        foreach(Image image in statBars){
            image.fillAmount = 0;
        }

        int seen = 0;
        int caught = 0;
        for (int i = 0; i < pokemonReferenceLib.pokemonDefaultLib.Length; i++){
            PokemonDefault mon = pokemonReferenceLib.pokemonDefaultLib[i];
            dexPlates[pokemonReferenceLib.pokemonDefaultLib[i].IDNumber].representsThisPokemon = mon;
            dexPlates[pokemonReferenceLib.pokemonDefaultLib[i].IDNumber].gameObject.SetActive(true);
            seen += LoadDexInfo.globalDexProgress[i] != DexStatus.Unknown ? 1 : 0;
            caught += LoadDexInfo.globalDexProgress[i] == DexStatus.Caught ? 1 : 0;
        }

        seenText.text = "Seen: " + seen;
        caughtText.text = "Caught: " + caught;
        locationText.text = "Area Unknown";
        spriteNameText.text = "???";
        eggGroupText.text = "Undiscovered";
        evYieldText.text = "Unknown";
    }

    public void DisableDexUI() {
        heightBox.text = "";
        weightBox.text = "";
        speciesBox.text = "";
        dexEntry.text = "Select a Pokémon to view its details.";
        dexSprite.sprite = mysterySprite;
        gameObject.SetActive(false);
        for (int i = 1; i < dexPlates.Length; i++){
            dexPlates[i].gameObject.SetActive(false);
        }
        type1box.gameObject.SetActive(false);
        type2box.gameObject.SetActive(false);
    }

    public void EnableDexUI() {
        this.gameObject.SetActive(true);
    }
}
