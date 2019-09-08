using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{ 
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

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {

        }
    }
}