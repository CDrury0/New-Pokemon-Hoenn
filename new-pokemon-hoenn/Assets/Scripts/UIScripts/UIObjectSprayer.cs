using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectSprayer : MonoBehaviour
{
    [SerializeField] RectTransform sprayParent;
    [SerializeField] GameObject toSpray;

    void Awake() {
        StartCoroutine(Spray());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator Spray() {
        GameObject instance = Instantiate(toSpray, sprayParent);
        RectTransform newTransform = instance.GetComponent<RectTransform>();
        newTransform.localPosition = new Vector3(
            sprayParent.rect.width / -2 - 100,
            Random.Range(sprayParent.rect.height / -2 + 10, sprayParent.rect.height / 2 - 10)
        );
        Vector3 newScale = new(newTransform.localScale.x * Random.Range(0.8f, 1.2f), newTransform.localScale.y);
        instance.transform.localScale = newScale;
        yield return new WaitForSeconds(Random.Range(0.02f, 0.05f));
        StartCoroutine(Spray());
    }
}
