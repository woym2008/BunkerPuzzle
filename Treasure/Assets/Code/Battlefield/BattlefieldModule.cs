using UnityEngine;
using System.Collections;
using Bunker.Module;

public class BattlefieldModule : LogicModule
{
    BattlefieldModule(string name) :base(name)
    {

    }
    public override void Create()
    {
        base.Create();
    }

    public override void Release()
    {
        base.Release();
    }

    protected override void OnModuleMessage(string msg, object[] args)
    {
        base.OnModuleMessage(msg, args);
    }
}
