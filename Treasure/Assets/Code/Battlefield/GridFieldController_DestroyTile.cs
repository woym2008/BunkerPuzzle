using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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
            return !_isWorking;
        }

        public override void Excute(params object[] objs)
        {
            base.Excute(objs);

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