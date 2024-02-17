using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderYStatic : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    void Awake() {
        // Set order in layer to the negative world-space position Y component
        spriteRenderer.sortingOrder = -(int)transform.position.y;
    }
}
