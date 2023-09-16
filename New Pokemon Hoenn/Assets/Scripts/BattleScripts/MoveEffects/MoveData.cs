using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//info common to every move
public class MoveData : MonoBehaviour, ICheckMoveFail, ICheckMoveSelectable
{
    public string moveName;
    [TextArea(3,3)] public string moveDescription;
    [SerializeField] private Pokemon.Type moveType;
    public bool typeFromWeather;
    public bool hiddenPowerType;
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
    public bool ignoresProtect;
    public MoveVisualData visualData;
    public MoveAccuracyData accuracyData;
    public const string FAIL = "But it failed!";

    public Pokemon.Type GetEffectiveMoveType(Pokemon user){
        if(typeFromWeather){
            return CombatSystem.Weather.typeFromWeather;
        }
        if(hiddenPowerType){
            return user.hiddenPowerType;
        }
        return moveType;
    }

    public Pokemon.Type GetEffectiveMoveType() {
        return moveType;
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
        //if a matching move is found, it means the user has already moved since being sent out
        if(onlyUsableFirstTurn && CombatSystem.MoveRecordList.FindRecordOfUserMove(user.pokemon) != null){
            return FAIL;
        }
        if(worksOnAsleep && target.pokemon.primaryStatus != PrimaryStatus.Asleep){
            return FAIL;
        }
        if(worksWhileAsleep && user.pokemon.primaryStatus != PrimaryStatus.Asleep){
            return FAIL;
        }
        if(FailAgainstProtect(user, target)){
            return target.GetName() + " protected itself!";
        }
        //fail if sound based move is used on soundproof target
        return null;
    }

    public static GameObject GetBaseMove(GameObject move){
        if(move == null){
            return null;
        }
        MultiTurnEffect multiTurn = move.GetComponent<MultiTurnEffect>();
        return multiTurn != null ? multiTurn.baseMove != null ? multiTurn.baseMove : move : move;
    }

    private bool FailAgainstProtect(BattleTarget user, BattleTarget target){
        if(ignoresProtect){
            return false;
        }
        if(targetType == TargetType.Ally || target == user){
            return false;
        }
        return target.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyProtect) != null;
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
    [Tooltip("Entry hazards ignore effects that would normally make a move miss, like semi-invulnerable states")]
    public bool entryHazard;
    public SemiInvulnerable hitsSemiInvulnerable;
    public Weather bypassOnWeather;
    public float hurtsIfMiss;

    public bool CheckMoveHit(MoveData moveData, BattleTarget user, BattleTarget target){
        if(moveData.targetType == TargetType.Self || moveData.targetType == TargetType.Ally || entryHazard){
            return true;
        }
        AppliedEffectInfo lockOnEffect = target.individualBattleModifier.appliedEffects.Find(e => e.effect is ApplyLockOn);
        if(lockOnEffect != null){
            lockOnEffect.effect.RemoveEffect(target, lockOnEffect);
            return true;
        }
        if(target.individualBattleModifier.semiInvulnerable == SemiInvulnerable.None || hitsSemiInvulnerable == target.individualBattleModifier.semiInvulnerable){
            if(cannotMissVulnerable){
                return true;
            }
            if(BypassOnWeather()){
                return true;
            }
            if(Random.Range(0f, 1f) / AccuracyMult(user.individualBattleModifier.statStages[5], target.individualBattleModifier.statStages[6]) <= accuracy){
                return true;
            }
        } 
        return false;
    }

    private bool BypassOnWeather(){
        return bypassOnWeather != null && bypassOnWeather == CombatSystem.Weather;
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
