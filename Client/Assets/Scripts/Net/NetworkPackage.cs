using System;
using System.Collections.Generic;
using UnityEngine;
using AsyncSocket;

public class NetworkPackage
{
    public CMD cmd;
    public List<string> list;
    public Action<DataBase> action;
}
