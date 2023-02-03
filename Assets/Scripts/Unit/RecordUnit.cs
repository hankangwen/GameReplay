using System;
using System.Collections;
using UnityEngine;


public class RecordUnit : MonoBehaviour
{
    public int UnitId;
    public int speed = 10;
    Vector3 lastDir = Vector3.zero;
    Vector3 curDir = Vector3.zero;
    protected Unit unit;
    public virtual void Awake()
    {
        UnitMgr.Instance.Register(UnitId, unit);
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
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
            GameMgr.Instance.RecordMove(unit, velocity);
        }
        lastDir.x = curDir.x;
        lastDir.y = curDir.y;
        lastDir.z = curDir.z;
    }
}
