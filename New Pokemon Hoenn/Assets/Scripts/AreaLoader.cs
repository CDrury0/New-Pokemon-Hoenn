using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : MonoBehaviour
{
    [SerializeField] private GameAreaManager areaEntered;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player")){
            StartCoroutine(areaEntered.LoadArea());
        }
    }
}
