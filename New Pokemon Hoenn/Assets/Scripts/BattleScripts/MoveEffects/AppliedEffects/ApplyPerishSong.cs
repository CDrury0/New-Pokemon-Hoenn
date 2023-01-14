using System.Collections;

public class ApplyPerishSong : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(CombatLib.Instance.moveFunctions.ChangeTargetHealth(target, -target.pokemon.stats[0]));
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s time is up!"));
            RemoveEffect(target, effectInfo);
            yield break;
        }
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " will faint in " + effectInfo.timer + " turn(s)!"));
        effectInfo.timer--;
    }

    void Awake(){
        message = "Everyone hearing the song will faint in 3 turns!";
    }
}
