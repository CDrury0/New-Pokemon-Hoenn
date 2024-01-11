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
        return p.pokemonDefault.evolutionData?.GetEvolved(p) != null;
    }

    private static PokemonDefault GetMonToEvolveInto(Pokemon p, ItemData usedEvoStone){
        int firstSlot = PlayerParty.Instance.playerParty.PartyIsFull();
        if(p.pokemonDefault.gift != null && firstSlot != -1){
            PlayerParty.Instance.playerParty.party[firstSlot] = new Pokemon(p.pokemonDefault.gift, p.level) {isShiny = p.isShiny};
        }
        
        if(usedEvoStone != null){
            return p.pokemonDefault.stoneEvolutions.Find(e => e.key == usedEvoStone).value;
        }

        return p.pokemonDefault.evolutionData?.GetEvolved(p);
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
