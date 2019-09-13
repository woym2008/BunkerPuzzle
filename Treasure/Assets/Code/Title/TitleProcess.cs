#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: TitleProcess
// 作    者：zhangfan
// 创建时间：2019/8/28 17:15:10
// 描    述：
// 版    本：
//-----------------------------------------------------------------------------
// Copyright (C) 2017-2019 零境科技有限公司
//-----------------------------------------------------------------------------
#endregion
using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;
using UnityEngine.SceneManagement;

namespace Bunker.Game
{
    public class TitleProcess : BasicProcess
    {
        CommonMonoBehaviour _titleLogicObject;
        TitleModule _titleModule;
        public override void Create()
        {
            base.Create();
            //加载资源
            
        }

        private void Update(float dt)
        {

        }

        public override void StartProcess(params object[] args)
        {
            base.StartProcess(args);
            //显示ui
            _titleLogicObject = MonoBehaviourHelper.CreateObject();
            _titleLogicObject.gameObject.name = "MainMenuRoot";
            _titleLogicObject.StartCoroutine(LoadMainMenuScene());
            //
            //ProcessManager.getInstance.Switch<BattlefieldProcess>();
        }

        IEnumerator LoadMainMenuScene()
        {
            yield return 0;

            var back = SceneManager.LoadSceneAsync("MainMenu");
            while(!back.isDone)
            {
                yield return 0;
            }

            _titleLogicObject.onupdate += Update;

            _titleModule = ModuleManager.getInstance.GetModule<TitleModule>();
            ModuleManager.getInstance.StartModule<TitleModule>();
        }

        public override void EndProcess()
        {
            _titleLogicObject.onupdate -= _titleModule.Update;
            ModuleManager.getInstance.StopModule<TitleModule>();
            base.EndProcess();
        }

        public override void Release()
        {
            base.Release();
        }
    }
}

