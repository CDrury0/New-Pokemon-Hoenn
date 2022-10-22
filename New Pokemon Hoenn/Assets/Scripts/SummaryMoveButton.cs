using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SummaryMoveButton : MonoBehaviour
{
    public RectTransform currentTransform;
    public MoveSO moveRepresented;
    public int[] ppValues = new int[2];
    public Text descriptionText;
    public Image categorySprite;
    public Text moveNameText;
    public Text movePPText;
    public Image moveTypeImage;
    public Text moveTypeText;
    public Text movePowerText;
    public Text moveAccuracyText;

    public void LoadMoveDescription()
    {
        if(moveRepresented != null)
        {
            descriptionText.text = moveRepresented.moveDescription;
        }
    }
}
