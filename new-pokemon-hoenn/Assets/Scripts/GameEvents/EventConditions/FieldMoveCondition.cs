using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/FieldMoveCondition")]
public class FieldMoveCondition : EventCondition, IStateDialogue
{
    [SerializeField] private GameObject requiredMove;
    private List<IStateDialogue> stringReplacers = new();

    public Dictionary<string, string> GetReplaceTable() {
        IEnumerable<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>();
        foreach(var replacer in stringReplacers)
            temp = temp.Union(replacer.GetReplaceTable());

        return temp.ToDictionary((i) => i.Key, (i) => i.Value);
    }

    public override bool IsConditionTrue() {
        stringReplacers.Add(requiredMove.GetComponent<MoveData>());
        var fieldMove = requiredMove.GetComponent<FieldMove>();
        if(fieldMove is null || !fieldMove.IsFieldUseEligible())
            return false;

        foreach(Pokemon p in PlayerParty.Instance.playerParty.party){
            if(p is not null && p.moves.Contains(requiredMove)){
                stringReplacers.Add(p);
                return true;
            }
        }
        return false;
    }
}
