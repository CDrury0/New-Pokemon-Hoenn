using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBall : ItemEffect
{
    [SerializeField] private bool guaranteedCapture;
    protected abstract float BallCatchRateMod(Pokemon user, Pokemon target);

    public override bool CanEffectBeUsed(Pokemon p){
        return CombatSystem.EnemyTrainer == null;
    }

    //this implementation assumes that only the player can use pokeballs
    protected override IEnumerator ItemEffectImplementation(Pokemon p, BattleHUD hudObj, System.Func<string, IEnumerator> messageOutput, int whichMove){
        Pokemon targetMon = CombatSystem.BattleTargets.Find(b => !b.teamBattleModifier.isPlayerTeam).pokemon;
        float catchRate = GetModifiedCatchRate(p, targetMon);
        if(!ShakeCheck(catchRate)){
            message = "Failed first shake check";
            yield break;
        }
        if(!ShakeCheck(catchRate)){
            message = "Failed second shake check";
            yield break;
        }
        if(!ShakeCheck(catchRate)){
            message = "Failed third shake check";
            yield break;
        }
        if(!ShakeCheck(catchRate)){
            message = "Failed fourth shake check";
            yield break;
        }
        message = "Caught! (Hypothetically)";
    }

    private bool ShakeCheck(float catchRate){
        if(guaranteedCapture){
            return true;
        }
        int result = (int)(1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / catchRate)));
        return Random.Range(0, 65536) < result;
    }

    private float GetModifiedCatchRate(Pokemon user, Pokemon target){
        float rate = RemainingHPMod(target.CurrentHealth, target.stats[0]);
        rate *= target.pokemonDefault.catchRate;
        rate *= BallCatchRateMod(user, target);
        rate *= StatusCatchRateMod(target.primaryStatus);
        rate *= 1.1f - (0.008f * target.level);
        return rate;
    }

    private float StatusCatchRateMod(PrimaryStatus status){
        return status switch {
            PrimaryStatus.Frozen => 2f,
            PrimaryStatus.Asleep => 2f,
            PrimaryStatus.Poisoned => 1.5f,
            PrimaryStatus.Paralyzed => 1.5f,
            PrimaryStatus.Burned => 1.5f,
            _ => 1f,
        };
    }

    private float RemainingHPMod(int currentHealth, int maxHealth){
        return (float)(3 * maxHealth - 2 * currentHealth) / (3 * maxHealth);
    }
}
