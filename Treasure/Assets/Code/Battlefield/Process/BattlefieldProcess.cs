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
        RobotManagerModule _robotManagerModule;
        BattleTurnsModule _battleTurnsModule;

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

            int level = (int)args[0];
            int area = (int)args[1];

            _battleLogicObject.StartCoroutine(LoadBattleScene(level, area));


        }

        public override void EndProcess()
        {
            //_battleLogicObject.onupdate -= _battleModule.Update;
            _battleLogicObject.StartCoroutine(RemoveScene());



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

            if (_robotManagerModule != null)
            {
                _robotManagerModule.Update(dt);
            }

            if (_battleTurnsModule != null)
            {
                _battleTurnsModule.Update(dt);
            }
        }

        IEnumerator RemoveScene()
        {

            //yield return new WaitForSeconds(.10f);

            ModuleManager.getInstance.StopModule<BattlefieldModule>();
            ModuleManager.getInstance.StopModule<BattlefieldCameraModule>();
            ModuleManager.getInstance.StopModule<BattlefieldInputModule>();
            ModuleManager.getInstance.StopModule<RobotManagerModule>();
            ModuleManager.getInstance.StopModule<BattleTurnsModule>();

            //var back = SceneManager.UnloadSceneAsync("Battlefield");
            //while (!back.isDone)
            //{
            //    yield return 0;
            //}

            _battleLogicObject.onupdate -= Update;

            GameObject.Destroy(_battleLogicObject);

            yield return 0;
        }

        IEnumerator LoadBattleScene(int level, int area)
        {
            yield return 0;

            var back = SceneManager.LoadSceneAsync("Battlefield");
            while(!back.isDone)
            {
                yield return 0;
            }

            _battleLogicObject.onupdate += Update;
            //-----------------

            //-----------------
            //
            _battleModule = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _battleModule.SelectLevel(area, level);

            ModuleManager.getInstance.StartModule<BattlefieldCameraModule>();
            ModuleManager.getInstance.StartModule<BattlefieldInputModule>();
            //UI mode 启动前置！
            ModuleManager.getInstance.StartModule<BattleUIModule>();

            ModuleManager.getInstance.StartModule<BattlefieldModule>();

            ModuleManager.getInstance.StartModule<RobotManagerModule>();
            ModuleManager.getInstance.StartModule<BattleTurnsModule>();


            _battleUIModule = ModuleManager.getInstance.GetModule<BattleUIModule>();
            _battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            _robotManagerModule = ModuleManager.getInstance.GetModule<RobotManagerModule>();
            _battleTurnsModule = ModuleManager.getInstance.GetModule<BattleTurnsModule>();

            _battleTurnsModule.NextTurn();

        }
    }
}

