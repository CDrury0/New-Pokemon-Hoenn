using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
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
        {new Vector3(0, 1, 0), 0},
        {new Vector3(1, 0, 0), 1},
        {new Vector3(0, -1, 0), 2},
        {new Vector3(-1, 0, 0), 3}
    };

    public IEnumerator AnimateMovement(Vector3 direction, float moveSpeed, bool willMove, bool isSprinting){
        Sprite[] spriteSet = isSprinting ? runSpriteSets[DirectionToArray[direction]] : walkSpriteSets[DirectionToArray[direction]];
        foreach(Sprite sp in spriteSet){
            playerSprite.sprite = sp;
            yield return new WaitForSeconds((1 / moveSpeed) / (spriteSet.Length + 1));
        }
        playerSprite.sprite = stillSprites[DirectionToArray[direction]];
    }

    void Awake(){
        walkSpriteSets = new Sprite[][] { walkUpSprites, walkRightSprites, walkDownSprites, walkLeftSprites };
        runSpriteSets = new Sprite[][] { runUpSprites, runRightSprites, runDownSprites, runLeftSprites };
    }
}
