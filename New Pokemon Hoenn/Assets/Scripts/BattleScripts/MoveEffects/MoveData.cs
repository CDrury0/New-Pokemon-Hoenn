using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//info common to every move
public class MoveData : MonoBehaviour, ICheckMoveFail, ICheckMoveSelectable
{
    public string moveName;
    [TextArea(3,3)] public string moveDescription;
    [SerializeField] private StatLib.Type moveType;
    public bool typeFromWeather;
    public int displayPower; //the power value shown in UI
    public enum Category{Physical, Special, Status};
    public Category category;
    public TargetType targetType;
    public int maxPP;
    public int priority;
    public bool pursuit;
    public bool soundBased;
    public bool focusPunch;
    public bool onlyUsableFirstTurn;
    public bool worksOnAsleep;
    public bool worksWhileAsleep;
    public bool cannotBeSnatched;
    public bool notReflectedByMagicCoat;
    public MultiTurnData multiTurnData;
    public MoveVisualData visualData;
    public MoveAccuracyData accuracyData;
    public const string FAIL = "But it failed!";

    public StatLib.Type GetEffectiveMoveType(){
        return typeFromWeather ? CombatLib.Instance.moveFunctions.GetMoveTypeFromWeather(CombatSystem.Weather) : moveType;
    }

    public List<GameObject> GetUnusableMoves(BattleTarget target){
        List<GameObject> unusableMoves = new List<GameObject>();
        if(target.pokemon.movePP[target.pokemon.moves.IndexOf(gameObject)] == 0){
            unusableMoves.Add(gameObject);
        }
        return unusableMoves;
    }

    public string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(onlyUsableFirstTurn && user.individualBattleModifier.lastUsedMove != null){
            return FAIL;
        }
        if(worksOnAsleep && target.pokemon.primaryStatus != PrimaryStatus.Asleep){
            return FAIL;
        }
        if(worksWhileAsleep && user.pokemon.primaryStatus != PrimaryStatus.Asleep){
            return FAIL;
        }
        //fail if sound based move is used on soundproof target
        return null;
    } 
}

[System.Serializable]
public class MultiTurnData
{
    public string specialText;
    public Weather skipChargingTurn;
    public GameObject useNext;
    [SerializeField] private GameObject baseMove;
    public SemiInvulnerable givesSemiInvulnerable;
    public int forcedToUseMax;
    public bool alwaysUseMaxTurns;
    public bool bideCharge;
    public bool confuseOnEnd;
    public bool mustRechargeAfter;

    public static GameObject GetBaseMove(MoveData data){
        return data.multiTurnData.baseMove != null ? data.multiTurnData.baseMove : data.gameObject;
    }
}

[System.Serializable]
public class MoveVisualData
{
    //animation clips, audio clips
}

[System.Serializable]
public class MoveAccuracyData
{
    public float accuracy;
    public bool cannotMissVulnerable;
    public SemiInvulnerable hitsSemiInvulnerable;
    public Weather bypassOnWeather;

    public bool CheckMoveHit(MoveData moveData, BattleTarget user, BattleTarget target, Weather weather){
        if(moveData.targetType == TargetType.Self || moveData.targetType == TargetType.Ally){
            return true;
        }
        AppliedEffectInfo lockOnEffect = target.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyLockOn);
        if(lockOnEffect != null){
            target.individualBattleModifier.appliedEffects.Remove(lockOnEffect);
            return true;
        } 
        if(cannotMissVulnerable && target.individualBattleModifier.semiInvulnerable == SemiInvulnerable.None){
            return true;
        }
        if(bypassOnWeather != Weather.None && bypassOnWeather == weather){
            return true;
        }
        if(Random.Range(0f, 1f) * AccuracyMult(user.individualBattleModifier.statStages[5], target.individualBattleModifier.statStages[6]) <= accuracy){
            return true;
        }
        return false;
    }

    private float AccuracyMult(int userAccStages, int targetEvaStages) 
    {
        int accMod = userAccStages - targetEvaStages;
        float accMult = (float)(Mathf.Abs(accMod) + 5f) / 5f;
        if (accMod < 0)
        {
            accMult = 1f / accMult;
        }
        return accMult;
    }
}
