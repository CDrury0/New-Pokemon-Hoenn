using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IFlashImage
{
    private const int NUM_FLASHES = 4;
    private const float INTERVAL_SECONDS = 0.08f;

    public IEnumerator DoImageFlash(Image img){
        Color startingColor = new Color(img.color.r, img.color.g, img.color.b, 1f);
        for (int i = 0; i < NUM_FLASHES; i++){
            yield return new WaitForSeconds(INTERVAL_SECONDS);
            img.color = Color.clear;
            yield return new WaitForSeconds(INTERVAL_SECONDS);
            img.color = startingColor;
        }
    }
}
