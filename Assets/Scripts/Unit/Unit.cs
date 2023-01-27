using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int UnitId;
    public int speed = 10;
    Vector3 lastDir = Vector3.zero;
    Vector3 curDir = Vector3.zero;
    Vector3 velocity;

    private void Awake()
    {
        UnitMgr.Instance.Register(UnitId, this);
    }

    private void FixedUpdate()
    {
        transform.position += velocity;
    }

    public void Move(float x, float z)
    {
        var playMode = GameMgr.Instance.PlayMode;
        if (playMode != PlayMode.Record)
            return;

        curDir.x = x;
        curDir.y = 0;
        curDir.z = z;
        var velocity = curDir * speed * Time.deltaTime;
        transform.position += velocity;

        if (curDir != lastDir)
        {
            GameMgr.Instance.RecordMove(this, velocity);
        }
        lastDir.x = curDir.x;
        lastDir.y = curDir.y;
        lastDir.z = curDir.z;
    }

    public void ReplayPos(PosCmd cmd)
    {
        transform.position = cmd.Pos.ToVector3();
        transform.eulerAngles = cmd.Dir.ToVector3();
    }

    public void ReplayMove(MoveCmd cmd)
    {
        var playMode = GameMgr.Instance.PlayMode;
        if (playMode != PlayMode.Replay)
            return;

        velocity = cmd.Velocity.ToVector3();
    }
}