using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Replayer : IPlay
{
    Queue<Frame> queframe = new Queue<Frame>();
    string filePath;
    int count;
    bool isPlaying;

    public void Init(string path)
    {
        filePath = path;
    }

    public void Play()
    {
        isPlaying = true;
        count = 1;
        queframe.Clear();

        string jsonStr = File.ReadAllText(filePath);
        var listFrame = JsonConvert.DeserializeObject<List<Frame>>(jsonStr);
        foreach (Frame frame in listFrame)
        {
            queframe.Enqueue(frame);
        }
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public void Update()
    {
        if (!isPlaying)
            return;
        if (queframe.Count == 0)
            return;
        
        count++;

        //返回队列顶部元素
        var frame = queframe.Peek();
        //没有到达关键帧则返回
        if (frame.Id > count)
            return;

        GameMgr.Instance.Replay(frame);
        queframe.Dequeue();
    }
}
