using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;

namespace Bunker.Game
{
    public class BattlefieldProcess : BasicProcess
    {
        CommonMonoBehaviour _battleLogicObject;

        BattlefieldModule _battleModule;
        public override void Create()
        {
            base.Create();


        }

        public override void Release()
        {
            base.Release();
        }

        public override void StartProcess(params object[] args)
        {
            base.StartProcess(args);

            _battleLogicObject = MonoBehaviourHelper.CreateObject();
            //_battleLogicObject.onupdate += Update;

            //
            _battleModule = ModuleManager.getInstance.GetModule("BattlefieldModule") as BattlefieldModule;
            _battleLogicObject.onupdate += _battleModule.Update;
        }

        public override void EndProcess()
        {
            _battleLogicObject.onupdate -= _battleModule.Update;
            ModuleManager.getInstance.StopModule<BattlefieldModule>();

            base.EndProcess();

        }

        private void Update(float dt)
        {

        }
    }
}

