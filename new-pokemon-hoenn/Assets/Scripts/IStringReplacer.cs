using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStringReplacer
{
    public Dictionary<string, string> GetReplaceTable();
}
