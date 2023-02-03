using System.Collections;
using UnityEngine;

public class ReplayUnit : RecordUnit
{
    CmdListener listener = new CmdListener();
    Vector3 velocity;
    public override void Awake()
    {
        base.Awake();
        //添加监听：指令类型，处理函数
        listener.Add<PosCmd>(CmdEnum.Pos, DoPosCmd);
        listener.Add<MoveCmd>(CmdEnum.Move, DoMoveCmd);
    }

    private void FixedUpdate()
    {
        transform.position += velocity;
    }

    /// <summary>
    /// 重新播放时用于重置数据
    /// </summary>
    public void ResetUnit()
    {
        velocity = Vector3.zero;
    }

    private void DoPosCmd(PosCmd posCmd)
    {
        transform.position = posCmd.Pos.ToVector3();
        transform.localEulerAngles = posCmd.Dir.ToVector3();
    }

    private void DoMoveCmd(MoveCmd cmd)
    {
        velocity = cmd.Velocity.ToVector3();
    }

    public void DoCmd(CmdEnum type, ICommand cmd)
    {
        var playMode = GameMgr.Instance.PlayMode;
        if (playMode != PlayMode.Replay)
            return;

        listener.Do(type, cmd);
    }
}
