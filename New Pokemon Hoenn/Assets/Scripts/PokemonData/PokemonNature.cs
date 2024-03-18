using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon Nature")]
public class PokemonNature : ScriptableObject
{
    public const float MOD_AMOUNT = 0.1f;
    [Tooltip("0 is attack; value 1 means boost, -1 means penalty")] [SerializeField] private int[] stats = new int[5];

    public float[] GetNatureModifiers(){
        float[] natureMods = new float[] { 1, 1, 1, 1, 1, 1 };
        for(int i = 1; i < natureMods.Length; i++){
            natureMods[i] += stats[i-1] * MOD_AMOUNT;
        }
        return natureMods;
    }
}
