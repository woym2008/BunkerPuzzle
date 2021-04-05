using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;
using UnityEngine.SceneManagement;

namespace Bunker.Game
{
    public class SelectLevelProcess : BasicProcess
    {
        public override void Create()
        {
            base.Create();
        }

        public override void StartProcess(params object[] args)
        {
            base.StartProcess(args);

            var commonnode = MonoBehaviourHelper.GetCommonObject();

            commonnode.StartCoroutine(LoadScene());

            //LevelManager.getInstance.LoadLevelFiles();
            //LevelManager.getInstance.Load();

            //var levels = LevelManager.getInstance.GetAreaLevels(LevelManager.getInstance.LastArea);

            //UIModule.getInstance.Open<AreaPanel>(levels, LevelManager.getInstance.LastArea);
        }

        public override void EndProcess()
        {
            UIModule.getInstance.Close<AreaPanel>();

            base.EndProcess();
        }

        IEnumerator LoadScene()
        {
            yield return 0;

            var back = SceneManager.LoadSceneAsync("LevelScene");
            while (!back.isDone)
            {
                yield return 0;
            }
            //-----------------
            //

            LevelManager.getInstance.LoadLevelFiles();
            LevelManager.getInstance.Load();

            var levels = LevelManager.getInstance.GetAreaLevels(LevelManager.getInstance.LastArea);

            UIModule.getInstance.Open<AreaPanel>(levels, LevelManager.getInstance.LastArea);

        }

    }
}

