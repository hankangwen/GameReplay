using System;
using System.Collections.Generic;
using UnityEngine;

public enum CmdEnum
{
    Pos,
    Move
}

public class ICommand
{
    public CmdEnum Type;
}

public class MoveCmd : ICommand
{
    public RVector3 Velocity;

    public MoveCmd()
    {
        Type = CmdEnum.Move;
    }
}

public class PosCmd : ICommand
{
    public RVector3 Pos;
    public RVector3 Dir;

    public PosCmd()
    {
        Type = CmdEnum.Pos;
    }
}

/// <summary>
/// 序列化Vector3
/// </summary>
public struct RVector3
{
    public float x;
    public float y;
    public float z;

    public RVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}