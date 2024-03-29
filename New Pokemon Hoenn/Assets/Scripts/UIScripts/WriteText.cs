using System.Collections;
using UnityEngine;
using TMPro;

public class WriteText : MonoBehaviour
{
    [SerializeField] private GameObject floatIndicator;
    public TextMeshProUGUI text;
    private bool skip;
    private IEnumerator wait;

    void OnDisable(){
        Skip();
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        skip = true;
    }

    public void Skip()
    {
        if (wait != null)
        {
            StopCoroutine(wait);
        }
        skip = true;
    }

    public IEnumerator WriteMessage(string message)
    {
        if(string.IsNullOrEmpty(message)){
            yield break;
        }

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

    public void WriteMessageInstant(string message){
        text.text = message;
        gameObject.SetActive(true);
    }

    public IEnumerator WriteMessageConfirm(string message, float forcedWait = 0f)
    {
        if (string.IsNullOrEmpty(message))
        {
            yield break;
        }

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
        yield return new WaitForSeconds(forcedWait);
        
        GameObject floatObj = Instantiate(floatIndicator, transform.position, Quaternion.identity, transform);
        floatObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(text.preferredWidth + 55, 0);

        yield return new WaitUntil(() => { return skip; });
        Destroy(floatObj);
        skip = false;
    }
}
