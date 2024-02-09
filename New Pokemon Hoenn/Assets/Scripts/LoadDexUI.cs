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
    public Transform plateListParent;
    public GameObject dexPlatePrefab;
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
        dexPlates = new LoadDexInfo[pokemonReferenceLib.pokemonDefaultLib.Count + 1];
        for (int i = 0; i < pokemonReferenceLib.pokemonDefaultLib.Count; i++){
            PokemonDefault mon = pokemonReferenceLib.pokemonDefaultLib[i];         
            LoadDexInfo currentPlate = Instantiate(dexPlatePrefab, plateListParent).GetComponent<LoadDexInfo>();
            currentPlate.representsThisPokemon = mon;
            dexPlates[mon.IDNumber] = currentPlate;
            seen += ReferenceLib.GlobalDexProgress[i] != DexStatus.Unknown ? 1 : 0;
            caught += ReferenceLib.GlobalDexProgress[i] == DexStatus.Caught ? 1 : 0;
        }

        // To sort alphabetically, simply set sibling index accordingly
        foreach(LoadDexInfo d in dexPlates){
            d?.gameObject.SetActive(true);
            d?.transform.SetSiblingIndex(d.representsThisPokemon.IDNumber);
        }

        seenText.text = "Seen: " + seen;
        caughtText.text = "Caught: " + caught;
        locationText.text = "Area Unknown";
        spriteNameText.text = "???";
        eggGroupText.text = "Undiscovered";
        evYieldText.text = "Unknown";
    }
}
