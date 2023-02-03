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
            OnInput(1);
        }
        else if (Input.GetKeyUp(KeyCode.F2))
        {
            OnInput(2);
        }
    }

    void OnInput(int type)
    {
        IPlayer player = PlayMode == PlayMode.Record ? recorder : replayer;
        if (type == 1)
        {
            player.Play();
        }
        else
        {
            player.Stop();
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

    public void ResetAllUnit()
    {
        var dictUnit = UnitMgr.Instance.GetAllUnit();
        foreach (var item in dictUnit)
        {
            item.Value.ResetUnit();
        }
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
        ICommand cmd = null;
        if (type == CmdEnum.Pos)
        {
            cmd = JsonConvert.DeserializeObject<PosCmd>(frame.Data);
        }
        else if (type == CmdEnum.Move)
        {
            cmd = JsonConvert.DeserializeObject<MoveCmd>(frame.Data);
        }
        if (cmd == null)
        {
            MonoHelper.Error($"指令反序列化失败 {frame.Id} {frame.Cmd}");
            return;
        }
        unit.DoCmd(type, cmd);
    }
}

public enum PlayMode
{
    Record,
    Replay,
}

public interface IPlayer
{
    public void Init(string fileName);
    public void Play();
    public void Stop();
    public void Update();
}
