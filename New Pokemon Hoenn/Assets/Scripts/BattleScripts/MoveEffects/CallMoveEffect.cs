using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CallMoveEffect : MoveEffect
{
    public List<GameObject> prohibitedMoves;

    protected void RemoveIllegalMoves(List<GameObject> usableMoves){
        usableMoves.RemoveAll(move => move == null);
        usableMoves.RemoveAll(move => move.GetComponent<MoveData>().moveName == gameObject.GetComponent<MoveData>().moveName);
        usableMoves.RemoveAll(move => prohibitedMoves.Contains(move));
    } 
}
