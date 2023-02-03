using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Replayer : IPlayer
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
        GameMgr.Instance.ResetAllUnit();
    }

    public void Stop()
    {
        isPlaying = false;
        GameMgr.Instance.ResetAllUnit();
    }

    public void Update()
    {
        if (!isPlaying)
            return;
        if (queframe.Count == 0)
            return;
        
        count++;

        //���ض��ж���Ԫ��
        var frame = queframe.Peek();
        //û�е���ؼ�֡�򷵻�
        if (frame.Id > count)
            return;

        GameMgr.Instance.Replay(frame);
        queframe.Dequeue();
    }
}
