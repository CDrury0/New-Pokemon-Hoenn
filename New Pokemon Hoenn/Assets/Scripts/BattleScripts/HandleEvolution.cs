using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEvolution : MonoBehaviour
{
    private bool CanMonEvolve(Pokemon p, EvoDetails evoDetails){
        if(p.level >= evoDetails.evolutionLevel){
            if(evoDetails.evolvesWithHeldItem != null && p.heldItem == evoDetails.evolvesWithHeldItem){
                return true;
            }
            if(p.friendship >= evoDetails.evolvesWithFriendship && evoDetails.evolvesWithFriendship != 0){
                return true;
            }
            //if no special conditions are met, the pokemon evolves exclusively based on level
            if(evoDetails.evolvesWithHeldItem == null && evoDetails.evolvesWithFriendship == 0){
                return true;
            }
        }
        return false;
    }

    private PokemonDefault GetMonToEvolveInto(Pokemon p, EvoDetails evoDetails){
        int firstSlot = PlayerParty.Instance.playerParty.PartyIsFull();
        if(evoDetails.shedinja != null && firstSlot != -1){
            //guarantee shininess in the event that nincada is shiny?
            PlayerParty.Instance.playerParty.party[firstSlot] = new Pokemon(evoDetails.shedinja, p.level);
        }
        if(evoDetails.evolvesFromGender != Gender.None){
            if(evoDetails.evolvesFromGender == Gender.Male){
                return evoDetails.firstOrMale;
            }
            else{
                return evoDetails.secondOrFemale;
            }
        }
        if(evoDetails.evolvesRandom){
            return (int)p.hiddenPowerType % 2 == 0 ? evoDetails.firstOrMale : evoDetails.secondOrFemale;
        }
        return evoDetails.firstOrMale;
    }

    private IEnumerator DoEvolutionScreen(){
        Debug.Log("epic evolution animation");
        yield break;
    }

    public IEnumerator EvolveMon(Pokemon p){
        EvoDetails evoDetails = p.pokemonDefault.evoDetails;
        if(CanMonEvolve(p, evoDetails)){
            PokemonDefault evolveInto = GetMonToEvolveInto(p, evoDetails);
            //change pokemon data
            yield return StartCoroutine(DoEvolutionScreen());
        }
    }
}
