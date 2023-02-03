using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : ReplayUnit
{
    public override void Awake()
    {
        SetUnit(this);
        base.Awake();
    }
}