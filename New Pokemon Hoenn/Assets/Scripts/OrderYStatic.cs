using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderYStatic : MonoBehaviour
{
    [Tooltip("If empty, automatically grabs the sprite renderer from this gameobject")]
    [SerializeField] protected SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetOrder();
    }

    protected void SetOrder() {
        // Set order in layer to the negative world-space position Y component
        spriteRenderer.sortingOrder = -(int)transform.position.y;
    }
}
