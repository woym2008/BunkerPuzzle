using UnityEngine;
using System.Collections;
using Bunker.Process;
using Bunker.Module;

namespace Bunker.Game
{
    public class EndMenuProcess : BasicProcess
    {
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

            UIModule.getInstance.Open<FinishPanel>();


        }

        public override void EndProcess()
        {


            base.EndProcess();

        }
    }

}

