using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI
{
    public abstract void ChooseAction(BattleTarget user, List<BattleTarget> participants);
}
