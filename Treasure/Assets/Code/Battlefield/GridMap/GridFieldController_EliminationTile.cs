using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Bunker.Game;

namespace Bunker.Game
{ 
    //手动消灭tile的controller，用于给道具使用
    public class GridFieldController_EliminationTile : GridFieldControllerBase
    {
        public override string ControllerType
        {
            get
            {
                return "ElminationTile";
            }
        }

        public override bool CanWaitInReadyStack
        {
            get
            {
                return true;
            }
        }

        bool _isWorking = false;

        public override bool IsFinish()
        {
            return !_isWorking;
        }

        public override void Excute()
        {
            var objs = _cacheobjs;

            base.Excute();

            _isWorking = true;

            var obj = objs[0];

            BaseTile[] points = (BaseTile[])objs;

            MonoBehaviourHelper.StartCoroutine(WaitforDestroyTile(points));
            //消灭

            //_isWorking = false;
        }

        IEnumerator WaitforDestroyTile(BaseTile[] datas)
        {
            //create effect

            var tempgrids = new List<BaseTile>();
            foreach(var g in datas)
            {
                tempgrids.Add(g);
            }

            _gridfield.EliminationGrids(tempgrids);
            yield return new WaitForSeconds(1.5f);

            _isWorking = false;           
        }

    }
}