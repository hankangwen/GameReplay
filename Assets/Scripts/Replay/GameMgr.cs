using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    public PlayMode PlayMode;
    public string FileName;
    Recorder recorder = new Recorder();
    Replayer replayer = new Replayer();

    protected override void Init()
    {
        base.Init();
        string filePath = Path.Combine(Application.streamingAssetsPath, $"Data/{FileName}.json");
        if (PlayMode == PlayMode.Record)
        {
            recorder.Init(filePath);
        }
        else
        {
            replayer.Init(filePath);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            recorder.Play();
        }
        else if (Input.GetKeyUp(KeyCode.F2))
        {
            recorder.Stop();
        }
        else if (Input.GetKeyUp(KeyCode.F11))
        {
            replayer.Play();
        }
        else if (Input.GetKeyUp(KeyCode.F12))
        {
            replayer.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (PlayMode == PlayMode.Record)
        {
            recorder.Update();
        }
        else
        {
            replayer.Update();
        }
    }

    public void RecordPos(Unit unit)
    {
        if (PlayMode != PlayMode.Record)
            return;

        var pos = unit.gameObject.transform.position;
        var dir = unit.gameObject.transform.eulerAngles;
        PosCmd cmd = new PosCmd()
        {
            Pos = pos.ToRVector3(),
            Dir = dir.ToRVector3(),
        };
        recorder.Record(cmd, unit.UnitId);
    }

    public void RecordAllPos()
    {
        var dictUnit = UnitMgr.Instance.GetAllUnit();
        foreach (var item in dictUnit)
        {
            RecordPos(item.Value);
        }
    }

    public void RecordMove(Unit unit, Vector3 velocity)
    {
        if (PlayMode != PlayMode.Record)
            return;

        MoveCmd cmd = new MoveCmd()
        {
            Velocity = velocity.ToRVector3(),
        };
        recorder.Record(cmd, unit.UnitId);
    }

    public void Replay(Frame frame)
    {
        MonoHelper.Debug($"执行指令：{frame.Id} {frame.Cmd}");
        var unit = UnitMgr.Instance.GetUnit(frame.Owner);
        if (unit == null)
        {
            MonoHelper.Error($"未找到Owner {frame.Id} {frame.Cmd}");
            return;
        }

        CmdEnum type = (CmdEnum)frame.Cmd;
        if (type == CmdEnum.Pos)
        {
            var cmd = JsonConvert.DeserializeObject<PosCmd>(frame.Data);
            unit.ReplayPos(cmd);
        }
        else if (type == CmdEnum.Move)
        {
            var cmd = JsonConvert.DeserializeObject<MoveCmd>(frame.Data);
            unit.ReplayMove(cmd);
        }
    }
}

public enum PlayMode
{
    Record,
    Replay,
}

public interface IPlay
{
    public void Init(string fileName);
    public void Play();
    public void Stop();
    public void Update();
}
