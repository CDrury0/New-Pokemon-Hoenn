using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InteractEvent : MonoBehaviour
{
    public abstract IEnumerator DoInteractEvent();
}
