using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EffectDamage : MoveEffect
{
    [SerializeField] private int _power;
    public int hitsMaxTimes;
    public bool makesContact;
}
