using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartyInfoBoxButtonContainer : MonoBehaviour
{
    [SerializeField] protected GameObject messageModalPrefab;
    public abstract void LoadActionButtons(Pokemon p);
}
