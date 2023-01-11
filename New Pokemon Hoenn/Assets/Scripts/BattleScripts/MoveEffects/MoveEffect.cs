using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveData))]
public abstract class MoveEffect : MonoBehaviour
{
    public bool applyToSelf;
    public string message;
    public abstract IEnumerator DoEffect(BattleTarget user, BattleTarget target, MoveData moveData);
    protected string ReplaceBattleMessage(BattleTarget user, BattleTarget target, MoveData moveData){
        string outputMessage = message.Replace("&userName", user.GetName());
        outputMessage = outputMessage.Replace("&targetName", target.GetName());
        outputMessage = outputMessage.Replace("&userPossessive", user.teamBattleModifier.teamPossessive);
        outputMessage = outputMessage.Replace("&targetPossessive", target.teamBattleModifier.teamPossessive);
        outputMessage = outputMessage.Replace("&moveName", moveData.moveName);
        return outputMessage;
    }
}
