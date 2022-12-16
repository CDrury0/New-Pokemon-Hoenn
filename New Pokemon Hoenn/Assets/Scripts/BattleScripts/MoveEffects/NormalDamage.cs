using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDamage : EffectDamage
{
    public float recoilDamage;
    public float absorbHealth;
    public int damageDealt; //needed for absorbHealth and recoilDamage calculations
    public bool spitUp;
    public bool facade;
    public bool revenge;
    public bool furyCutter;
    public bool highCritRate;
    public bool bonusFromCurl;
    public bool bonusLikeRollout;
    public bool cannotKO;
    public bool bonusAgainstMinimize;
    public SemiInvulnerable bonusAgainstSemiInvulnerable;
    public PrimaryStatus bonusAgainstStatus;
    public bool curesBonusStatus;
    public bool payback;

    public override IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        yield return StartCoroutine(base.NormalDamageMethod(user, target, moveData, 10 /*replace with power*/));
    }
}
