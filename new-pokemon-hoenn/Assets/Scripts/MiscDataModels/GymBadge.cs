using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymBadge : ScriptableObject
{
    [SerializeField] private string _badgeName;
    public string BadgeName { get => _badgeName; }

    [SerializeField] private Sprite _badgeSprite;
    public Sprite BadgeSprite { get => _badgeSprite; }
}
