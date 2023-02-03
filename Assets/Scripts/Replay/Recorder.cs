using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Recorder : IPlayer
{
    List<Frame> listFrame = new List<Frame>();
    int count;
    string filePath;
    bool isPlaying;

    public void Init(string path)
    {
        filePath = path;
    }

    public void Play()
    {
        count = 1;
        listFrame.Clear();
        isPlaying = true;

        GameMgr.Instance.RecordAllPos();
    }

    public void Stop()
    {
        isPlaying = false;
        string json = JsonConvert.SerializeObject(listFrame);
        MonoHelper.Debug($"记录帧数据：{json}");
        var dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(filePath, json);
    }

    public void Update()
    {
        if (!isPlaying)
            return;

        count++;
    }

    public void Record(ICommand command, int unitId)
    {
        if (!isPlaying)
            return;

        var frame = new Frame()
        {
            Id = count,
            Owner = unitId,
            Cmd = (int)command.Type,
            Data = JsonConvert.SerializeObject(command),
        };
        listFrame.Add(frame);

        MonoHelper.Debug($"记录指令：{count} {command.Type}");
    }
}
