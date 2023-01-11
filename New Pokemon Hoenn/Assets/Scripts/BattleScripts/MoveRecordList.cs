using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRecordList
{
    private List<MoveRecord> moveRecords = new List<MoveRecord>();
    public class MoveRecord{
        public Pokemon user;
        public Pokemon target;
        public GameObject moveUsed;

        public MoveRecord(Pokemon user, Pokemon target, GameObject moveUsed){
            this.user = user;
            this.target = target;
            this.moveUsed = moveUsed;
        }
    }

    public void AddRecord(Pokemon user, Pokemon target, GameObject moveUsed){
        moveRecords.Add(new MoveRecord(user, target, moveUsed));
    }

    public MoveRecord FindRecordLastAttacker(Pokemon targetOfAttack){
        return moveRecords.FindLast(moveRecord => moveRecord.target == targetOfAttack && moveRecord.target != moveRecord.user);
    }

    public MoveRecord FindRecordLastUsedBy(Pokemon user){
        return moveRecords.FindLast(moveRecord => moveRecord.user == user);
    }

    public MoveRecord FindRecordMirrorMove(Pokemon target, List<GameObject> prohibitedMoves){
        //return the record where a move was most recently used against the supplied target by someone other than the target itself, and where the move is not found in a list of prohibited moves
        return moveRecords.FindLast(record => record.target == target && record.user != target && !prohibitedMoves.Contains(record.moveUsed));
    }

    public MoveRecord FindRecordOfUserMove(Pokemon user){
        return moveRecords.Find(record => record.user == user);
    }
}
