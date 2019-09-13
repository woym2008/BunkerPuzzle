using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class BattleUIModule : LogicModule
    {
        public BattleUIModule() : base(typeof(BattleUIModule).ToString())
        {
            
        }
        public BattleUIModule(string name) :base(name)
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
            UIModule.getInstance.Open<MainMenuPanel>();
        }

        public override void OnStop()
        {
            UIModule.getInstance.Close<MainMenuPanel>();
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