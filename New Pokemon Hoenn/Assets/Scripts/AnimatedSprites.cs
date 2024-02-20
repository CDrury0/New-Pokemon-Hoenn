using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprites : MonoBehaviour
{
    public int state;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("Start with 'up' state and rotate clockwise")]
    [SerializeField] private Sprite[] sprites;

    void Update() {
        try{
            spriteRenderer.sprite = sprites[state];
        }
        catch(System.IndexOutOfRangeException){
            Debug.LogWarning(gameObject.name + ": A sprite corresponding to the state set does not exist");
        }
    }
}
