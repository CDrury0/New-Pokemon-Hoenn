using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] stillSprites;
    [SerializeField] private Sprite[] walkUpSprites;
    [SerializeField] private Sprite[] walkRightSprites;
    [SerializeField] private Sprite[] walkDownSprites;
    [SerializeField] private Sprite[] walkLeftSprites;
    private Sprite[][] walkSpriteSets;
    [SerializeField] private Sprite[] runUpSprites;
    [SerializeField] private Sprite[] runRightSprites;
    [SerializeField] private Sprite[] runDownSprites;
    [SerializeField] private Sprite[] runLeftSprites;
    private Sprite[][] runSpriteSets;
    private Dictionary<Vector3, int> DirectionToArray = new Dictionary<Vector3, int>() {
        {Vector3.up, 0},
        {Vector3.right, 1},
        {Vector3.down, 2},
        {Vector3.left, 3}
    };

    public void FaceDirection(Vector3 direction){
        spriteRenderer.sprite = stillSprites[DirectionToArray[direction]];
    }

    public IEnumerator AnimateMovement(Vector3 direction, float moveSpeed, bool isMoving, bool isSprinting){
        Sprite[] spriteSet = isSprinting ? runSpriteSets[DirectionToArray[direction]] : walkSpriteSets[DirectionToArray[direction]];
        foreach(Sprite sp in spriteSet){
            spriteRenderer.sprite = sp;
            yield return new WaitForSeconds((1 / moveSpeed) / (spriteSet.Length + 1));
        }
        spriteRenderer.sprite = stillSprites[DirectionToArray[direction]];
    }

    void Awake(){
        walkSpriteSets = new Sprite[][] { walkUpSprites, walkRightSprites, walkDownSprites, walkLeftSprites };
        runSpriteSets = new Sprite[][] { runUpSprites, runRightSprites, runDownSprites, runLeftSprites };
    }

    void Reset() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
