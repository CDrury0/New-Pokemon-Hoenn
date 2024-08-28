using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvolutionData/EvolutionDataGender")]
public class EvolutionDataGender : EvolutionData
{
    [SerializeField] private List<DynamicDictionary<Gender, PokemonDefault>.Entry> entries;
    
    public override PokemonDefault GetEvolved(Pokemon p){
        return CheckLevel(p) ? entries.Find(e => e.key == p.gender).value : null;
    }
}
