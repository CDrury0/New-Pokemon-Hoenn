using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDurationEffect{Safeguard, StatChangeImmune, Reflect, LightScreen}  //individual/team-BattleModifier contains List of these, compare this.durationEffect with teambattlemodifier.durationEffect
public class ApplyTeamDurationEffect : MoveEffect
{
    public TeamDurationEffect durationEffect;
    public Weather weatherSet;

    public override IEnumerator DoEffect(BattleTarget user, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
