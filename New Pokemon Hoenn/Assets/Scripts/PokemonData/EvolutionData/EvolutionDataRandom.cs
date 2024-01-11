using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvolutionData/EvolutionDataRandom")]
public class EvolutionDataRandom : EvolutionData
{
    [SerializeField] private List<PokemonDefault> entries;

    public override PokemonDefault GetEvolved(Pokemon p){
        return CheckLevel(p) ? entries[(int)p.hiddenPowerType % entries.Count] : null;
    }
}
