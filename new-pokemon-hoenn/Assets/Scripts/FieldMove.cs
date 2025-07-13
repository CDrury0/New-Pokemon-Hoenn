using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMove : MonoBehaviour
{
    [SerializeField] private GymBadge _requiredBadge;
    public GymBadge RequiredBadge { get => _requiredBadge; }

    [SerializeField] private bool _cannotBeForgotten;
    public bool CannotBeForgotten { get => _cannotBeForgotten; }

    public bool IsFieldUseEligible() {
        return ReferenceLib.PlayerBadges.Contains(RequiredBadge);
    }
}
