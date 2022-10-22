using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDexUI : MonoBehaviour
{
    [SerializeField]
    public Color[] typeColors = new Color[18];
    public static Color[] typeColorsLib = new Color[18];
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

    private void Awake()
    {
        for (int i = 0; i < typeColors.Length; i++)
        {
            typeColorsLib[i] = typeColors[i];
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        foreach(Image image in statBars)
        {
            image.fillAmount = 0;
        }

        int seen = 0;
        int caught = 0;
        for (int i = 1; i < dexPlates.Length; i++)
        {
            dexPlates[pokemonReferenceLib.pokemonDefaultLib[i].IDNumber].representsThisPokemon = pokemonReferenceLib.pokemonDefaultLib[i];
            dexPlates[pokemonReferenceLib.pokemonDefaultLib[i].IDNumber].gameObject.SetActive(true);
            seen += GlobalGameEvents.globalDexProgress[i] != DexStatus.Unknown ? 1 : 0;
            caught += GlobalGameEvents.globalDexProgress[i] == DexStatus.Caught ? 1 : 0;
        }

        seenText.text = "Seen: " + seen;
        caughtText.text = "Caught: " + caught;
        locationText.text = "Area Unknown";
        spriteNameText.text = "???";
        eggGroupText.text = "Undiscovered";
        evYieldText.text = "Unknown";
    }

    public void DisableDexUI()
    {
        heightBox.text = "";
        weightBox.text = "";
        speciesBox.text = "";
        dexEntry.text = "Select a Pokémon to view its details.";
        dexSprite.sprite = mysterySprite;
        this.gameObject.SetActive(false);
        for (int i = 1; i < dexPlates.Length; i++)
        {
            dexPlates[i].gameObject.SetActive(false);
        }
        GlobalGameEvents.blockPlayerInputOverworld = false;
        type1box.gameObject.SetActive(false);
        type2box.gameObject.SetActive(false);
    }

    public static Color GetTypeColor(StatLib.Type type)
    {
        switch (type)
        {
            case StatLib.Type.Normal:
                return typeColorsLib[0];
            case StatLib.Type.Fighting:
                return typeColorsLib[1];
            case StatLib.Type.Flying:
                return typeColorsLib[2];
            case StatLib.Type.Poison:
                return typeColorsLib[3];
            case StatLib.Type.Ground:
                return typeColorsLib[4];
            case StatLib.Type.Rock:
                return typeColorsLib[5];
            case StatLib.Type.Bug:
                return typeColorsLib[6];
            case StatLib.Type.Ghost:
                return typeColorsLib[7];
            case StatLib.Type.Steel:
                return typeColorsLib[8];
            case StatLib.Type.Fire:
                return typeColorsLib[9];
            case StatLib.Type.Water:
                return typeColorsLib[10];
            case StatLib.Type.Grass:
                return typeColorsLib[11];
            case StatLib.Type.Electric:
                return typeColorsLib[12];
            case StatLib.Type.Psychic:
                return typeColorsLib[13];
            case StatLib.Type.Ice:
                return typeColorsLib[14];
            case StatLib.Type.Dragon:
                return typeColorsLib[15];
            case StatLib.Type.Dark:
                return typeColorsLib[16];
            case StatLib.Type.Fairy:
                return typeColorsLib[17];
            default:
                return typeColorsLib[0];
        }
    }

    public void EnableDexUI()
    {
        this.gameObject.SetActive(true);
        GlobalGameEvents.blockPlayerInputOverworld = true;
    }


}
