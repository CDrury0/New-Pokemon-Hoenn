using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderYDynamic : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    void Update() {
        spriteRenderer.sortingOrder = -(int)transform.position.y;
    }
}
