using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvolutionData/EvolutionDataFriendship")]
public class EvolutionDataFriendship : EvolutionData
{
    [SerializeField] private int friendshipReq;
    [SerializeField] private PokemonDefault evolveInto;

    public override PokemonDefault GetEvolved(Pokemon p){
        return CheckLevel(p) && p.Friendship >= friendshipReq ? evolveInto : null;
    }
}
