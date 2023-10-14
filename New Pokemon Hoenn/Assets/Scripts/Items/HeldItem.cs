using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemLogic))]
public class HeldItem : MonoBehaviour
{
    [SerializeField] private List<ItemEffect> heldEffect;
    
    //determine when/how the logic for held item should trigger/be executed
}
