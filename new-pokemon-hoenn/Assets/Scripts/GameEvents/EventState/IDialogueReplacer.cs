using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueReplacer
{
    public Dictionary<string, string> GetReplaceTable();
}
