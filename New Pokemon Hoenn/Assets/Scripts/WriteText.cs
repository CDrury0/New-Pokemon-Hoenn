using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WriteText : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI text;
    private bool skip;
    private IEnumerator wait;

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        skip = true;
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (wait != null)
        {
            StopCoroutine(wait);
        }
        skip = true;
    }

    public IEnumerator WriteMessage(string message)
    {
        wait = Wait(2.5f);
        text.text = "";
        gameObject.SetActive(true);
        skip = false;
        foreach (char c in message)
        {
            text.text += c;
            yield return new WaitForSeconds(0.02f);

            if (skip)
            {
                text.text = message;
                skip = false;
                break;
            }
        }
        StartCoroutine(wait);
        yield return new WaitUntil(() => { return skip; });
        skip = false;
    }

    public IEnumerator WriteMessageConfirm(string message)
    {
        text.text = "";
        gameObject.SetActive(true);
        skip = false;
        foreach (char c in message)
        {
            text.text += c;
            yield return new WaitForSeconds(0.02f);

            if (skip)
            {
                text.text = message;
                skip = false;
                break;
            }
        }
        yield return new WaitUntil(() => { return skip; });
        skip = false;
    }
}
