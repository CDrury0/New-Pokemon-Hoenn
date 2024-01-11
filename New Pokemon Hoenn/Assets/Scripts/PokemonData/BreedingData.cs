using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BreedingData
{
    [SerializeField] private PokemonDefault thisParent;
    [SerializeField] private PokemonDefault normalChild;
    [SerializeField] private PokemonDefault alternateChild;
    [SerializeField] private ItemData heldItem;
    [SerializeField] [Tooltip("If true, the child species will be either father or mother")] private bool random;
    [SerializeField] private PokemonDefault requiredParent;

    /// <returns>The reference data of the child species, or null if no valid child can be produced</returns>
    public PokemonDefault GetChildPokemon(Pokemon father){
        return null;
    }
}
