using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string powerUpDescription;
    public Color powerUpColor;
    public Sprite sprite;

    public abstract void apply(GameObject player);
}
