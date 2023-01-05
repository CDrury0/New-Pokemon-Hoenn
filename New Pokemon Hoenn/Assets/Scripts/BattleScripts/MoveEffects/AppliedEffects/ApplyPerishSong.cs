using System.Collections;

public class ApplyPerishSong : ApplyIndividualEffect, IApplyEffect
{
    public IEnumerator DoAppliedEffect(BattleTarget target, AppliedEffectInfo effectInfo)
    {
        if(effectInfo.timer == 0){
            yield return StartCoroutine(target.battleHUD.healthBar.SetHealthBar(target.pokemon, -target.pokemon.stats[0]));
            target.pokemon.CurrentHealth -= target.pokemon.stats[0];
            yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + "'s time is up!"));
            RemoveEffect(target, effectInfo);
            yield break;
        }
        yield return StartCoroutine(CombatLib.Instance.WriteBattleMessage(target.GetName() + " will faint in " + effectInfo.timer + " turn(s)!"));
        effectInfo.timer--;
    }
}
