using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;

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

            LevelManager.getInstance.LoadLevelFiles();
            LevelManager.getInstance.Load();

            var levels = LevelManager.getInstance.GetAreaLevels(LevelManager.getInstance.LastArea);

            UIModule.getInstance.Open<AreaPanel>(levels, LevelManager.getInstance.LastArea);
        }

        public override void EndProcess()
        {
            UIModule.getInstance.Close<AreaPanel>();

            base.EndProcess();
        }
    }
}

