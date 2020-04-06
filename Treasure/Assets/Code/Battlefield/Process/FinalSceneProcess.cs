using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;
using UnityEngine.SceneManagement;

namespace Bunker.Game
{
    public class FinalSceneProcess : BasicProcess
    {
        CommonMonoBehaviour _finalObject;
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
            if (_finalObject == null)
                _finalObject = MonoBehaviourHelper.CreateObject();
            _finalObject.gameObject.name = "FinalObject";
            _finalObject.StartCoroutine(LoadMainMenuScene());
        }

        IEnumerator LoadMainMenuScene()
        {
            yield return 0;

            var back = SceneManager.LoadSceneAsync("FinalScene");

            while (!back.isDone)
            {
                yield return 0;
            }
        }

        public override void EndProcess()
        {
            base.EndProcess();
        }

        public override void Release()
        {
            base.Release();
        }
    }
}
