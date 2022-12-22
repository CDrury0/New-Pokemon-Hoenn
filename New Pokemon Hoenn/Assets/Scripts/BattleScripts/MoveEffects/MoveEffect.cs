using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveData))]
public abstract class MoveEffect : MonoBehaviour
{
    public bool applyToSelf;
    public abstract IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData);
}
