using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvolutionData/EvolutionDataHeldItem")]
public class EvolutionDataHeldItem : EvolutionData
{
    [SerializeField] private List<DynamicDictionary<ItemData, PokemonDefault>.Entry> entries;
    public override PokemonDefault GetEvolved(Pokemon p){
        return CheckLevel(p) ? entries.Find(e => e.key == p.heldItem).value : null;
    }
}
