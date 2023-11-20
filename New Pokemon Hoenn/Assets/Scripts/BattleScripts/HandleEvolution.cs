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

    private static bool CanMonEvolve(Pokemon p){
        EvoDetails evoDetails = p.pokemonDefault.evoDetails;
        //everstone?
        if(evoDetails.evolutionLevel > 0 && p.level >= evoDetails.evolutionLevel){
            if(evoDetails.evolvesWithHeldItem != null && p.heldItem == evoDetails.evolvesWithHeldItem){
                return true;
            }
            if(p.Friendship >= evoDetails.evolvesWithFriendship && evoDetails.evolvesWithFriendship != 0){
                return true;
            }
            //if no special conditions are met, the pokemon evolves exclusively based on level
            if(evoDetails.evolvesWithHeldItem == null && evoDetails.evolvesWithFriendship == 0){
                return true;
            }
        }
        return false;
    }

    private static PokemonDefault GetMonToEvolveInto(Pokemon p, ItemData usedEvoStone){
        EvoDetails evoDetails = p.pokemonDefault.evoDetails;
        if(usedEvoStone != null){
            return evoDetails.GetEvoStoneMatch(usedEvoStone);
        }

        int firstSlot = PlayerParty.Instance.playerParty.PartyIsFull();
        if(evoDetails.shedinja != null && firstSlot != -1){
            PlayerParty.Instance.playerParty.party[firstSlot] = new Pokemon(evoDetails.shedinja, p.level) {isShiny = p.isShiny};
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
        yield return StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(() => {
            background.SetActive(true);
            monSprite.sprite = previousEvo;
            textObj.WriteMessageInstant("");
        }));
        yield return StartCoroutine(textObj.WriteMessageConfirm("Huh?"));
        yield return StartCoroutine(textObj.WriteMessageConfirm(previousNickname + " is evolving!"));
        monSprite.sprite = p.frontSprite; //replace with evolution animation
        yield return StartCoroutine(textObj.WriteMessageConfirm(previousNickname + " evolved into " + p.pokemonName + "!"));
    }

    public IEnumerator EvolveMon(Pokemon p, ItemData usedEvoStone = null){
        if(CanMonEvolve(p) || usedEvoStone != null){
            PokemonDefault evolveInto = GetMonToEvolveInto(p, usedEvoStone);
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
            if (WillEvolutionOccur()){
                yield break;
            }
            //Only do this transition if there will be no more evolutions done with this instance of HandleEvolution
            yield return StartCoroutine(OverlayTransitionManager.Instance.TransitionCoroutine(() => {
                background.SetActive(false);
                //If a party menu if found, disable it to skip additional pointless transition (ButtonContainerInventoryUse.UseItem is exited early)
                GameObject.FindGameObjectWithTag("PartyMenu")?.SetActive(false);
                //Set the combatScreen inactive, since any time an evolution would occur a battle will never be returned to anyway
                CombatLib.Instance.combatScreen.gameObject.SetActive(false);
            }));
        }
    }

    public IEnumerator DoEvolutions(){
        for (int i = 0; i < didLevelUpLastBattle.Count; i++){
            yield return StartCoroutine(EvolveMon(didLevelUpLastBattle[i]));
        }
        ClearMarkedLevelUps();
    }

    /// <param name="p">providing an argument causes only that Pokemon to be considered</param>
    public static bool WillEvolutionOccur(Pokemon p = null){
        if(p != null){
            return CanMonEvolve(p);
        }
        for(int i = 0; i < didLevelUpLastBattle.Count; i++){
            Pokemon temp = didLevelUpLastBattle[i];
            if(CanMonEvolve(temp)){
                Debug.Log(temp.nickName + " can evolve");
                return true;
            }
        }
        return false;
    }
}
