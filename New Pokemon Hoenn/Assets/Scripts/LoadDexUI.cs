using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LoadDexUI : MonoBehaviour
{
    public ColorSO typeColors;
    public Color[] statColors = new Color[6];
    public Image[] statBars = new Image[6];
    public GameObject type1box;
    public GameObject type2box;
    public TextMeshProUGUI heightBox;
    public TextMeshProUGUI weightBox;
    public TextMeshProUGUI speciesBox;
    public TextMeshProUGUI dexEntry;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI spriteNameText;
    public TextMeshProUGUI eggGroupText;
    public TextMeshProUGUI evYieldText;
    public TextMeshProUGUI seenText;
    public TextMeshProUGUI caughtText;
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
        dexPlates = new LoadDexInfo[pokemonReferenceLib.pokemonDefaultLib.Count];
        for (int i = 0; i < dexPlates.Length; i++){
            PokemonDefault mon = pokemonReferenceLib.pokemonDefaultLib[i];         
            LoadDexInfo currentPlate = Instantiate(dexPlatePrefab, plateListParent).GetComponent<LoadDexInfo>();
            currentPlate.representsThisPokemon = mon;
            currentPlate.alphaOrder = i + 1;
            currentPlate.gameObject.SetActive(true);
            dexPlates[i] = currentPlate;
            seen += ReferenceLib.GlobalDexProgress[i + 1] != DexStatus.Unknown ? 1 : 0;
            caught += ReferenceLib.GlobalDexProgress[i + 1] == DexStatus.Caught ? 1 : 0;
        }
        OrderPlatesByID();

        seenText.text = "Seen: " + seen;
        caughtText.text = "Caught: " + caught;
        locationText.text = "Area Unknown";
        spriteNameText.text = "???";
        eggGroupText.text = "Undiscovered";
        evYieldText.text = "Unknown";
    }

    public void OrderPlatesByID() {
        dexPlates = dexPlates.OrderBy(item => item.representsThisPokemon.IDNumber).ToArray();
        ReindexPlates();
    }

    public void OrderPlatesByName() {
        dexPlates = dexPlates.OrderBy(item => item.representsThisPokemon.pokemonName).ToArray();
        ReindexPlates();
    }

    private void ReindexPlates() {
        for(int i = 0; i < dexPlates.Length; i++){
            dexPlates[i].transform.SetSiblingIndex(i);
        }
    }
}
