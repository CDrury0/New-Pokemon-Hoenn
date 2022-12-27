using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI //: MonoBehaviour enables use as component on trainer game objects (temporarily disabled to allow instantiation via script)
{
    public abstract void ChooseAction(BattleTarget user, List<BattleTarget> participants);
}
