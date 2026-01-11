using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/FieldMoveCondition")]
public class FieldMoveCondition : EventCondition, IStateDialogue, IStateFeature
{
    [SerializeField] private GameObject requiredMove;
    private Tuple<Pokemon, MoveData> members = new(null, null);

    public Dictionary<string, string> GetReplaceTable() {
        IEnumerable<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>();
        temp = temp.Union(members?.Item1?.GetReplaceTable() ?? new());
        temp = temp.Union(members?.Item2?.GetReplaceTable() ?? new());
        return temp.ToDictionary((i) => i.Key, (i) => i.Value);
    }

    public Sprite GetSprite() => members.Item1.GetSprite();

    public AudioClip GetSound() => members.Item1.GetSound();

    public override bool IsConditionTrue() {
        members = new(members.Item1, requiredMove.GetComponent<MoveData>());
        var fieldMove = requiredMove.GetComponent<FieldMove>();
        if(fieldMove is null || !fieldMove.IsFieldUseEligible())
            return false;

        foreach(Pokemon p in PlayerParty.Party.members){
            if(p.moves.Contains(requiredMove)){
                members = new(p, members.Item2);
                return true;
            }
        }
        return false;
    }
}
