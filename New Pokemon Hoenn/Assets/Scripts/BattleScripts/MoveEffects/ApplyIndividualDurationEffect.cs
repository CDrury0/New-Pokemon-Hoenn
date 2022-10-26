using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IndividualDurationEffect {Curled, Protect, Curse, Encore, Identified, Taunt, Torment, Disable, Endure, LeechSeed, CenterOfAttention, Minimize, Ingrain, LockOn, Rage, Nightmare, Snatch, MagicCoat, Drowsy}
public class ApplyIndividualDurationEffect : ApplyEffect
{
    public bool applyToSelf;
    public IndividualDurationEffect individualEffect;

    public override IEnumerator DoAppliedEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoEffect(BattleTarget user, MoveData moveData)
    {
        throw new System.NotImplementedException();
    }
}
