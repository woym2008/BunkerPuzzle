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
            //if(_titleModule != null) _titleModule.Update(dt);
            if(Input.GetKeyDown(KeyCode.Backspace))
            {
                SaveLoader.getInstance.ClearGameProgress();
            }
        }

        public override void StartProcess(params object[] args)
        {
            base.StartProcess(args);
            //显示ui
            if(_titleLogicObject == null)
                _titleLogicObject = MonoBehaviourHelper.CreateObject();
            _titleLogicObject.gameObject.name = "MainMenuRoot";
            _titleLogicObject.StartCoroutine(LoadMainMenuScene());
            //
            //ProcessManager.getInstance.Switch<BattlefieldProcess>();

        }

        IEnumerator LoadMainMenuScene()
        {
            yield return 0;

            //var back = SceneManager.LoadSceneAsync("MainMenu");
            var back = SceneManager.LoadSceneAsync("MainMenu2D");

            while (!back.isDone)
            {
                yield return 0;
            }


            _titleModule = ModuleManager.getInstance.GetModule<TitleModule>();
            ModuleManager.getInstance.StartModule<TitleModule>();

            _titleLogicObject.onupdate += _titleModule.Update;
            _titleLogicObject.onupdate += Update;
        }

        public override void EndProcess()
        {
            _titleLogicObject.onupdate -= Update;
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

