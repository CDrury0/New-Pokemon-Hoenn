using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStringReplacer
{
    public abstract Dictionary<string, string> GetReplaceTable();
    public abstract string ApplyReplacements(string subject);
}
