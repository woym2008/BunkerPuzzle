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

        BattleControllerModule _controllerModule;
        //add by wwh 2021-4-4
        ShopUIModule _shopUIModule;
        //add by wwh 2021-5-13
        StarredModule _starredModule;

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
            _battleLogicObject = MonoBehaviourHelper.CreateObject("BattlefieldRoot");

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

            if (_controllerModule != null)
            {
                _controllerModule.Update(dt);
            }

            if (_robotManagerModule != null)
            {
                _robotManagerModule.Update(dt);
            }

            if (_battleTurnsModule != null)
            {
                _battleTurnsModule.Update(dt);
            }
            //TEST SHOW SHOP! add by wwh
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetShopVisiable(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetShopVisiable(false);
            }
        }

        public void SetShopVisiable(bool b)
        {
            if (b)
            {
                _shopUIModule.DisplayShopUIPanel();
                _battleUIModule.HideBattleUIPanel();
            }
            else
            {
                _shopUIModule.HideShopUIPanel();
                _battleUIModule.DisplayBattleUIPanel();

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
            ModuleManager.getInstance.StopModule<ShopUIModule>();
            ModuleManager.getInstance.StopModule<StarredModule>();

            //masktile管理
            ModuleManager.getInstance.StopModule<MaskTileModule>();

            //var back = SceneManager.UnloadSceneAsync("Battlefield");
            //while (!back.isDone)
            //{
            //    yield return 0;
            //}

            _battleLogicObject.onupdate -= Update;

            MissionManager.getInstance.OnMissionValueHandler -= OnMissionValue;

            //GameObject.Destroy(_battleLogicObject);

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
            ModuleManager.getInstance.StartModule<BattleTurnsModule>();
            ModuleManager.getInstance.StartModule<ShopUIModule>();


            //masktile管理
            ModuleManager.getInstance.StartModule<MaskTileModule>();

            //此处载入关卡数据mapdata
            ModuleManager.getInstance.StartModule<BattlefieldModule>();
            ModuleManager.getInstance.StartModule<RobotManagerModule>();
            ModuleManager.getInstance.StartModule<StarredModule>();



            _battleUIModule = ModuleManager.getInstance.GetModule<BattleUIModule>();
            _battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            _robotManagerModule = ModuleManager.getInstance.GetModule<RobotManagerModule>();
            _battleTurnsModule = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
            _shopUIModule = ModuleManager.getInstance.GetModule<ShopUIModule>();
            _controllerModule = ModuleManager.getInstance.GetModule<BattleControllerModule>();
            _starredModule = ModuleManager.getInstance.GetModule<StarredModule>();
            //这里尝试载入一下道具
            SaveLoader.getInstance.LoadPlayerCurItems(area);

            //加上mission回调
            MissionManager.getInstance.OnMissionValueHandler += OnMissionValue;

            _battleTurnsModule.NextTurn();

        }

        //--------------------------------------------------------
        void OnMissionValue(int value)
        {
            switch(value)
            {
                case MissionManager.Mission_Success:
                    {
                        ProcessManager.getInstance.Switch<EndMenuProcess>(EndMenuProcess.END_GAME_WIN);
                    }
                    break;
                case MissionManager.Mission_Failure:
                    {
                        ProcessManager.getInstance.Switch<EndMenuProcess>(EndMenuProcess.END_GAME_LOSE);
                    }
                    break;
            }
        }
    }
}

