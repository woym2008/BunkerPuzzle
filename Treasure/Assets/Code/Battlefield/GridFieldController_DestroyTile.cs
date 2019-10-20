using UnityEngine;
using System.Collections;
using Bunker.Game;

namespace Bunker.Game
{ 
    //手动消灭tile的controller，用于给道具使用
    public class GridFieldController_DestroyTile : GridFieldControllerBase
    {
        public override string ControllerType
        {
            get
            {
                return "DestroyTile";
            }
        }

        bool _isWorking = false;

        public override bool IsFinish()
        {
            return base.IsFinish();
        }

        public override void Excute(params object[] objs)
        {
            base.Excute(objs);

            _isWorking = true;

            GridPos[] points = (GridPos[])objs[0];

            //消灭

            _isWorking = false;
        }
    }
}