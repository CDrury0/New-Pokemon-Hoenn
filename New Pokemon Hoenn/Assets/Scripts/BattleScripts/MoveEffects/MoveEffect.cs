using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveData))]
public abstract class MoveEffect : MonoBehaviour
{
    public string message;
    public abstract IEnumerator DoEffect(BattleTarget user, MoveData moveData);
}
