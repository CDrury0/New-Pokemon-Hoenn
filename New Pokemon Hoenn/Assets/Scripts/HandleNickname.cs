using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandleNickname : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] TMP_InputField inputField;
    public GameObject bg;
    private Pokemon toGiveNickname;
    private string input;
    private bool proceed;

    public void SetInput(){
        input = inputField.text;
        if(string.IsNullOrWhiteSpace(input)){
            buttonText.text = "No Nickname";
            return;
        }
        buttonText.text = "Set Nickname";
    }

    public void Click(){
        toGiveNickname.nickName = !string.IsNullOrWhiteSpace(input) ? input : toGiveNickname.pokemonName;
        proceed = true;
    }

    public IEnumerator WaitForClick(Pokemon p, string prompt = null){
        toGiveNickname = p;
        promptText.text = prompt ?? "Give a nickname to the captured " + p.pokemonName + "?";
        bg.SetActive(true);
        yield return new WaitUntil(() => proceed);
    }
}
