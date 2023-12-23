using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBall : ItemEffect
{
    [SerializeField] private bool guaranteedCapture;
    protected abstract float BallCatchRateMod(Pokemon user, Pokemon target);

    public override bool CanEffectBeUsed(Pokemon p) {
        return CombatSystem.EnemyTrainer == null;
    }

    //this implementation assumes that only the player can use pokeballs
    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove) {
        Pokemon targetMon = CombatSystem.BattleTargets.Find(b => !b.teamBattleModifier.isPlayerTeam).pokemon;
        float catchRate = GetModifiedCatchRate(p, targetMon);
        //animation of ball being thrown
        if(!ShakeCheck(catchRate)){
            message = "The Pokémon broke free!";
            yield break;
        }
        //every shake check after the first should have a "roll" animation before it
        if(!ShakeCheck(catchRate)){
            message = "Aww, it appeared to be caught!";
            yield break;
        }
        if(!ShakeCheck(catchRate)){
            message = "Aargh! Almost had it!";
            yield break;
        }
        if(!ShakeCheck(catchRate)){
            message = "Shoot! It was so close!";
            yield break;
        }

        yield return StartCoroutine(messageOutput("Gotcha! " + targetMon.nickName + " was caught!"));

        //handle nickname
        GameObject nicknameObj = Instantiate(CombatLib.Instance.combatSystem.nicknameObjPrefab);
        HandleNickname handleNickname = nicknameObj.GetComponentInChildren<HandleNickname>(true);
        yield return StartCoroutine(handleNickname.WaitForClick(targetMon));
        Destroy(nicknameObj);

        //show dex entry?

        //handle add to party / box
        bool placedInParty = PlayerParty.Instance.GivePlayerPokemon(targetMon);
        string captureMessage = targetMon.nickName + (placedInParty ? " was added to the party" : " was sent to the PC");
        yield return StartCoroutine(messageOutput(captureMessage));
        CombatSystem.BattleEndSignal = true;
    }

    private bool ShakeCheck(float catchRate) {
        if(guaranteedCapture){
            return true;
        }
        //formula copied from bulbapedia. i don't even want to know...
        int result = (int)(1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / catchRate)));
        return Random.Range(0, 65536) < result;
    }

    private float GetModifiedCatchRate(Pokemon user, Pokemon target) {
        float rate = RemainingHPMod(target.CurrentHealth, target.stats[0]);
        rate *= target.pokemonDefault.catchRate;
        rate *= BallCatchRateMod(user, target);
        rate *= StatusCatchRateMod(target.primaryStatus);
        rate *= 1.1f - (0.008f * target.level);
        return rate;
    }

    private float StatusCatchRateMod(PrimaryStatus status) {
        return status switch {
            PrimaryStatus.Frozen => 2f,
            PrimaryStatus.Asleep => 2f,
            PrimaryStatus.Poisoned => 1.5f,
            PrimaryStatus.Paralyzed => 1.5f,
            PrimaryStatus.Burned => 1.5f,
            _ => 1f,
        };
    }

    private float RemainingHPMod(int currentHealth, int maxHealth) {
        return (float)(3 * maxHealth - 2 * currentHealth) / (3 * maxHealth);
    }
}
