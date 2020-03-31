using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;

namespace Bunker.Game
{
    public class EndMenuProcess : BasicProcess
    {
        public const int END_GAME_NULL = 0;
        public const int END_GAME_WIN = 1;
        public const int END_GAME_LOSE = 2;
        public int end_state = END_GAME_NULL;
        public FinishPanel panel;

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

            end_state = (int)args[0];
            //

            panel = UIModule.getInstance.Open<FinishPanel>();
            //将参数传入
            panel.SetCommandMood(end_state);

        }

        public override void EndProcess()
        {
            base.EndProcess();
        }
    }

}

