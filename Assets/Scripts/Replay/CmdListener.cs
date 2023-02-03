using System;
using System.Collections.Generic;
/// <summary>
/// 命令监听
/// </summary>
public class CmdListener
{
    Dictionary<CmdEnum, Delegate> commands = new Dictionary<CmdEnum, Delegate>();

    /// <summary>
    /// 添加监听
    /// </summary>
    public void Add<T>(CmdEnum type, Action<T> cmdCB) where T : ICommand
    {
        if (commands.ContainsKey(type))
        {
            MonoHelper.Error($"重复添加监听 {type}");
            return;
        }
        commands.Add(type, cmdCB);
    }

    /// <summary>
    /// 执行监听方法
    /// </summary>
    public void Do(CmdEnum type, ICommand cmd)
    {
        if (!commands.TryGetValue(type, out Delegate callback))
        {
            MonoHelper.Error($"未添加命令处理函数 {type}");
            return;
        }
        
        callback.DynamicInvoke(cmd);
    }
}

