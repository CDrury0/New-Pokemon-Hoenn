using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvolutionData/EvolutionDataLevel")]
public class EvolutionDataLevel : EvolutionData
{
    [SerializeField] private PokemonDefault evolveInto;

    public override PokemonDefault GetEvolved(Pokemon p){
        return CheckLevel(p) ? evolveInto : null;
    }
}
