using System;
using System.Collections.Generic;

public class Frame
{
    public int Id;
    public int Owner;
    public int Cmd;
    //json无法序列化嵌套obj，需要将obj转成string使用
    public string Data;
}

