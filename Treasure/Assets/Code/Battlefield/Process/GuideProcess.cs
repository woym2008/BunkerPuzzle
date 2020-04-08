using Bunker.Game;
using Bunker.Module;
using Bunker.Process;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuideProcess : BasicProcess
{
    CommonMonoBehaviour _battleLogicObject;

    GuideModule _guideModule;
    BattlefieldInputModule _battleInputModule;

    BattleUIModule _battleUIModule;
    //RobotManagerModule _robotManagerModule;
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

        _battleLogicObject.StartCoroutine(LoadBattleScene());

    }

    public override void EndProcess()
    {
        base.EndProcess();
        _battleLogicObject.StartCoroutine(RemoveScene());
    }

    private void Update(float dt)
    {
        if (_battleInputModule != null)
        {
            _battleInputModule.Update(dt);
        }

        if (_battleUIModule != null)
        {
            _battleUIModule.Update(dt);
        }

        if (_guideModule != null)
        {
            _guideModule.Update(dt);
        }

        //if (_robotManagerModule != null)
        //{
        //    _robotManagerModule.Update(dt);
        //}

        if (_battleTurnsModule != null)
        {
            _battleTurnsModule.Update(dt);
        }
    }



    IEnumerator LoadBattleScene()
    {
        yield return 0;

        var back = SceneManager.LoadSceneAsync("Battlefield");
        while (!back.isDone)
        {
            yield return 0;
        }

        _battleLogicObject.onupdate += Update;
        //-----------------

        //-----------------
        //
        UIModule.getInstance.ClearAll();
        var battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
        if (battleInputModule != null)
        {
            battleInputModule.Rest();
        }

        LevelManager.getInstance.SetCurrentLevel(0, 1);

        ModuleManager.getInstance.StartModule<BattlefieldCameraModule>();
        ModuleManager.getInstance.StartModule<BattlefieldInputModule>();
        //UI mode 启动前置！
        ModuleManager.getInstance.StartModule<BattleUIModule>();
        ModuleManager.getInstance.StartModule<BattleTurnsModule>();
        //此处载入关卡数据mapdata
        ModuleManager.getInstance.StartModule<GuideModule>();
        //ModuleManager.getInstance.StartModule<RobotManagerModule>();
        ModuleManager.getInstance.StartModule<BattleControllerModule>();


        _battleUIModule = ModuleManager.getInstance.GetModule<BattleUIModule>();
        _battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
        //_robotManagerModule = ModuleManager.getInstance.GetModule<RobotManagerModule>();
        _battleTurnsModule = ModuleManager.getInstance.GetModule<BattleTurnsModule>();

        //这里尝试载入一下道具
        //SaveLoader.getInstance.LoadPlayerCurItems(area);
        //加上mission回调
        MissionManager.getInstance.OnMissionValueHandler += OnMissionValue;

        _battleTurnsModule.NextTurn();

    }

    IEnumerator RemoveScene()
    {

        //yield return new WaitForSeconds(.10f);
        ModuleManager.getInstance.StopModule<BattleControllerModule>();
        ModuleManager.getInstance.StopModule<GuideModule>();
        ModuleManager.getInstance.StopModule<BattlefieldCameraModule>();
        ModuleManager.getInstance.StopModule<BattlefieldInputModule>();
        //ModuleManager.getInstance.StopModule<RobotManagerModule>();
        ModuleManager.getInstance.StopModule<BattleTurnsModule>();

        //var back = SceneManager.UnloadSceneAsync("Battlefield");
        //while (!back.isDone)
        //{
        //    yield return 0;
        //}
        MissionManager.getInstance.OnMissionValueHandler -= OnMissionValue;

        _battleLogicObject.onupdate -= Update;

        GameObject.Destroy(_battleLogicObject);

        yield return 0;
    }

    //--------------------------------------------------------
    void OnMissionValue(int value)
    {
        switch (value)
        {
            case MissionManager.Mission_Success:
                {
                    SaveLoader.getInstance.SaveGameCurProgress(
                       0,
                       1
                    );
                    //LevelManager.getInstance.SetCurrentLevel(1, 1);
                    //ProcessManager.getInstance.Switch<BattlefieldProcess>(LevelManager.getInstance.CurLevel, LevelManager.getInstance.CurArea);
                    ProcessManager.getInstance.Switch<SelectLevelProcess>();
                }
                break;
            case MissionManager.Mission_Failure:
                {
                    //ProcessManager.getInstance.Switch<EndMenuProcess>(EndMenuProcess.END_GAME_LOSE);
                }
                break;
        }
    }
}
