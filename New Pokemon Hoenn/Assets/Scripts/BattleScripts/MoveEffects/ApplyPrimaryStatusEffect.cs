using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPrimaryStatusEffect : MoveEffect, ICheckMoveFail
{
    public PrimaryStatus statusInflicted;
    public bool toxic;
    public float chance;
    public bool powder;
    public bool ignoresExistingCondition;
    public bool triAttack;

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        PrimaryStatus status = statusInflicted;
        if(triAttack){
            int rand = Random.Range(1, 4);
            switch(rand){
                case 1:
                status = PrimaryStatus.Frozen;
                break;
                case 2:
                status = PrimaryStatus.Burned;
                break;
                case 3:
                status = PrimaryStatus.Paralyzed;
                break;
            }
        }

        if(Random.Range(0f, 1f) <= chance){
            if(ImmuneToStatus(status, target, powder)){
                yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " is immune to the status condition!"));
                yield break;
            }
            if(!ignoresExistingCondition && target.pokemon.primaryStatus != PrimaryStatus.None){
                yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " is already " + target.pokemon.primaryStatus.ToString() + "!"));
            }
            else{
                target.pokemon.primaryStatus = status;
                target.pokemon.toxic = toxic;
                SetSleepCounter(status, target);
                yield return StartCoroutine(PrintStatusMessage(status, target));
                target.battleHUD.SetBattleHUD(target.pokemon);
            }
        }
    }

    private void SetSleepCounter(PrimaryStatus status, BattleTarget target){
        if(status == PrimaryStatus.Asleep){
            target.pokemon.sleepCounter = applyToSelf ? 2 : Random.Range(1, 5);
        }
    }

    private IEnumerator PrintStatusMessage(PrimaryStatus status, BattleTarget target){
        string message = target.GetName();
        if(status == PrimaryStatus.Asleep){
            message += " was put to Sleep!";
        }
        else if(toxic){
            message += " was badly Poisoned!";
        }
        else{
            message += " was " + status.ToString() + "!";
        }
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(message));
    }

    public static bool ImmuneToStatus(PrimaryStatus status, BattleTarget target, bool powder){
        if(powder && target.pokemon.IsThisType(Pokemon.Type.Grass)){
            return true;
        }
        if(target.teamBattleModifier.teamEffects.Find(e => e.effect.durationEffect == TeamDurationEffect.Safeguard) != null){
            return true;
        }
        if(status == PrimaryStatus.Poisoned){
            if(target.pokemon.IsThisType(Pokemon.Type.Poison) || target.pokemon.IsThisType(Pokemon.Type.Steel)){
                return true;
            }
        }
        else if(status == PrimaryStatus.Paralyzed){
            if(target.pokemon.IsThisType(Pokemon.Type.Electric)){
                return true;
            }
        }
        else if(status == PrimaryStatus.Asleep){
            //check for vital spirit, insomnia
        }
        else if(status == PrimaryStatus.Burned){
            if(target.pokemon.IsThisType(Pokemon.Type.Fire)){
                return true;
            }
        }
        else if(status == PrimaryStatus.Frozen){
            if(target.pokemon.IsThisType(Pokemon.Type.Ice)){
                return true;
            }
        }
        return false;
    }

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(moveData.category == MoveData.Category.Status && chance == 1f && ImmuneToStatus(statusInflicted, target, powder)){
            return "It doesn't affect " + target.GetName() + "...";
        }
        return null;
    }
}
