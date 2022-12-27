using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//info common to every move
public class MoveData : MonoBehaviour
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

    public StatLib.Type GetEffectiveMoveType(){
        return typeFromWeather ? CombatLib.Instance.moveFunctions.GetMoveTypeFromWeather(CombatSystem.Weather) : moveType;
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

    public bool CheckMoveHit(MoveData data, BattleTarget user, BattleTarget target, Weather weather){ 
        return true;
    }
}
