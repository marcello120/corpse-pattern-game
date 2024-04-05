using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CorpseConfig 
{
    public int key;
    public Sprite corpse;

    public CorpseConfig(int key, Sprite corpse)
    {
        this.key = key;
        this.corpse = corpse;
    }
}
