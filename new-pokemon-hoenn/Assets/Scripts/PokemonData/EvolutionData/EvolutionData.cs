using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EvolutionData : ScriptableObject
{
    [SerializeField] [Tooltip("0 indicates no level requirement")] protected int levelReq;

    /// <returns>The reference data of the pokemon to evolve into, or null if ineligible for evolution</returns>
    public abstract PokemonDefault GetEvolved(Pokemon p);

    protected bool CheckLevel(Pokemon p){
        return p.level >= levelReq || levelReq == 0;
    }
}
