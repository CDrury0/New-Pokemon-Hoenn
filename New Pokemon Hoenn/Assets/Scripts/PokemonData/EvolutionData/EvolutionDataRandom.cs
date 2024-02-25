using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvolutionData/EvolutionDataRandom")]
public class EvolutionDataRandom : EvolutionData
{
    [SerializeField] private List<PokemonDefault> entries;

    public override PokemonDefault GetEvolved(Pokemon p){
        if(!CheckLevel(p)){
            return null;
        }
        return entries[ReferenceLib.Instance.typeList.IndexOf(p.hiddenPowerType) + 1 % entries.Count];
    }
}
