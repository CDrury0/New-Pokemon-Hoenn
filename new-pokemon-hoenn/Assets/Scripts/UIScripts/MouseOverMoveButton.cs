using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverMoveButton : MonoBehaviour, IPointerEnterHandler
{
    public Text moveDescriptionText;
    public MoveData moveData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        moveDescriptionText.text = moveData.moveDescription;
    }
}
