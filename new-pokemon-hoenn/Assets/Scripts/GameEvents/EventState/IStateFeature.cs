using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateFeature
{
    public Sprite GetSprite();

    public AudioClip GetSound();
}
