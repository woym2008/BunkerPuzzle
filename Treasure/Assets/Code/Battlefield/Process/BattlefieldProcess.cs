using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;
using UnityEngine.SceneManagement;

namespace Bunker.Game
{
    public class BattlefieldProcess : BasicProcess
    {
        CommonMonoBehaviour _battleLogicObject;

        BattlefieldModule _battleModule;
        BattlefieldInputModule _battleInputModule;

        BattleUIModule  _battleUIModule;

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
            _battleLogicObject.gameObject.name = "BattlefieldRoot";

            _battleLogicObject.StartCoroutine(LoadBattleScene());


        }

        public override void EndProcess()
        {
            //_battleLogicObject.onupdate -= _battleModule.Update;
            _battleLogicObject.StopCoroutine(RemoveScene());

            GameObject.Destroy(_battleLogicObject);

            base.EndProcess();

        }

        private void Update(float dt)
        {
            if(_battleInputModule != null)
            {
                _battleInputModule.Update(dt);
            }

            if(_battleUIModule != null)
            {
                _battleUIModule.Update(dt);
            }

            if(_battleModule != null)
            {
                _battleModule.Update(dt);
            }
        }

        IEnumerator RemoveScene()
        {
            var back = SceneManager.UnloadSceneAsync("Battlefield");
            while (!back.isDone)
            {
                yield return 0;
            }
            ModuleManager.getInstance.StopModule<BattlefieldModule>();
            ModuleManager.getInstance.StopModule<BattlefieldCameraModule>();
            ModuleManager.getInstance.StopModule<BattlefieldInputModule>();

            _battleLogicObject.onupdate -= Update;


        }

        IEnumerator LoadBattleScene()
        {
            yield return 0;

            var back = SceneManager.LoadSceneAsync("Battlefield");
            while(!back.isDone)
            {
                yield return 0;
            }

            _battleLogicObject.onupdate += Update;

            //
            _battleModule = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            //_battleLogicObject.onupdate += _battleModule.Update;
            ModuleManager.getInstance.StartModule<BattlefieldModule>();
            ModuleManager.getInstance.StartModule<BattlefieldCameraModule>();
            ModuleManager.getInstance.StartModule<BattlefieldInputModule>();
            ModuleManager.getInstance.StartModule<BattleUIModule>();


            _battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            _battleUIModule = ModuleManager.getInstance.GetModule<BattleUIModule>();

            //_battleLogicObject.onupdate += _battleModule.Update;
        }
    }
}

