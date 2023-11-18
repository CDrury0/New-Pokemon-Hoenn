using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleEvolution : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private Image monSprite;
    [SerializeField] private WriteText textObj;
    [SerializeField] private GameObject learnMoveScreenPrefab;
    private static List<Pokemon> didLevelUpLastBattle = new List<Pokemon>();

    public static void MarkLevelUp(Pokemon p){
        didLevelUpLastBattle.Add(p);
    }

    public static void ClearMarkedLevelUps(){
        didLevelUpLastBattle = new List<Pokemon>();
    }

    private bool CanMonEvolve(Pokemon p, EvoDetails evoDetails){
        //everstone?
        if(p.level >= evoDetails.evolutionLevel){
            if(evoDetails.evolvesWithHeldItem != null && p.heldItem == evoDetails.evolvesWithHeldItem){
                return true;
            }
            if(p.Friendship >= evoDetails.evolvesWithFriendship && evoDetails.evolvesWithFriendship != 0){
                return true;
            }
            //if no special conditions are met, the pokemon evolves exclusively based on level
            if(evoDetails.evolvesWithHeldItem == null && evoDetails.evolvesWithFriendship == 0 && evoDetails.firstOrMale != null){
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
        if(evoDetails.evolvesFromGender){
            return p.gender == Gender.Male ? evoDetails.firstOrMale : evoDetails.secondOrFemale;            
        }
        if(evoDetails.evolvesRandom){
            return (int)p.hiddenPowerType % 2 == 0 ? evoDetails.firstOrMale : evoDetails.secondOrFemale;
        }
        return evoDetails.firstOrMale;
    }

    private IEnumerator DoEvolutionScreen(Sprite previousEvo, string previousNickname, Pokemon p){
        background.SetActive(true);
        monSprite.sprite = previousEvo;
        yield return StartCoroutine(textObj.WriteMessageConfirm("Huh?"));
        yield return StartCoroutine(textObj.WriteMessageConfirm(previousNickname + " is evolving!"));
        monSprite.sprite = p.frontSprite; //replace with evolution animation
        yield return StartCoroutine(textObj.WriteMessageConfirm(previousNickname + " evolved into " + p.pokemonName + "!"));
    }

    public IEnumerator EvolveMon(Pokemon p){
        EvoDetails evoDetails = p.pokemonDefault.evoDetails;
        if(CanMonEvolve(p, evoDetails)){
            PokemonDefault evolveInto = GetMonToEvolveInto(p, evoDetails);
            Sprite previousEvo = p.frontSprite;
            string previousNickname = p.nickName;
            p.Evolve(evolveInto);
            yield return StartCoroutine(DoEvolutionScreen(previousEvo, previousNickname, p));
            GameObject moveToLearn = LearnMoveScreen.GetValidMoveToLearn(p);
            if(moveToLearn != null){
                LearnMoveScreen learnScreen = Instantiate(learnMoveScreenPrefab).GetComponent<LearnMoveScreen>();
                yield return StartCoroutine(learnScreen.DoLearnMoveScreen(p, moveToLearn, (string message) => textObj.WriteMessageConfirm(message)));
                Destroy(learnScreen.gameObject);
            }
            background.SetActive(false);
        }
    }

    public IEnumerator DoEvolutions(){
        for (int i = 0; i < didLevelUpLastBattle.Count; i++){
            yield return StartCoroutine(EvolveMon(didLevelUpLastBattle[i]));
        }
        ClearMarkedLevelUps();
    }
}
