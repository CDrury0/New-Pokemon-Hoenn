using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryMovePlate : MonoBehaviour
{
    public Button button;
    [SerializeField] private Sprite[] categorySprites;
    [SerializeField] private Image categorySprite;
    [SerializeField] private TextMeshProUGUI moveName;
    [SerializeField] private TextMeshProUGUI ppText;
    [SerializeField] private Image typeSprite;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private string moveDescription;

    public void SetMoveInfo(int currentPP, int maxPP, MoveData moveData, Pokemon p) {
        categorySprite.sprite = categorySprites[(int)moveData.category];
        moveName.text = moveData.moveName;
        ppText.text = "PP: " + currentPP + " / " + maxPP;
        typeSprite.color = moveData.GetEffectiveMoveType(p).typeColor;
        typeText.text = moveData.GetEffectiveMoveType(p).name;
        powerText.text = "Power: " + (moveData.displayPower == 0 ? "---" : moveData.displayPower.ToString());
        accuracyText.text = "Accuracy: " + (moveData.accuracyData.accuracy == 0 ? "---" : (moveData.accuracyData.accuracy * 100).ToString());
        moveDescription = moveData.moveDescription;
    }

    public void SetDescriptionText() {
        descriptionText.text = moveDescription;
    }
}
