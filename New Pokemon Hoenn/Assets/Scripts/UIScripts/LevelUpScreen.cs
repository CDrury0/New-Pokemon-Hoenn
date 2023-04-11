using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpScreen : MonoBehaviour
{
    private bool _proceed;
    private bool Proceed {
        get {
            if(_proceed){
                _proceed = false;
                return true;
            }
            return _proceed;
        }
        set {
            _proceed = value;
        }
    }

    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private TextMeshProUGUI[] oldStatText;
    [SerializeField] private TextMeshProUGUI[] newStatText;

    public void SetProceed(){
        Proceed = true;
    }

    public IEnumerator DoLevelUpScreen(int[] oldStats, int[] newStats, string pokemonName) {
        nicknameText.text = pokemonName;
        for (int i = 0; i < oldStatText.Length; i++){
            oldStatText[i].text = oldStats[i].ToString() + " >";
            newStatText[i].text = " " + newStats[i].ToString();
        }
        background.SetActive(true);
        yield return new WaitUntil(() => Proceed);
        background.SetActive(false);
    }
}
