using UnityEngine;
using System.Collections;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    public class RobotThief : RobotBase
    {
        Vector3Int[] DirList = { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left };
        public override void OnPrepareMove()
        {
            //update cur tile
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var g = m.Field.GetGrid(transform.position) as BaseGrid;
            if (g != null)
            {
                SetToGird(g);

            }
            else
            {
                Debug.Log("RobotThief OnPrepareMove ERROR!");
            }
            //
            base.OnPrepareMove();
        }
        public override void OnFinishMove()
        {
            //recalc new grid XY
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _pos = m.Field.ClampGridPos(_pos.x + _dir.x , _pos.y - _dir.y); // here need use -
            var g = m.Field.GetGrid(_pos.x, _pos.y) as BaseGrid;
            this.transform.parent = g.Node.transform;
            //set order
            var sr = this.GetComponentInChildren<SpriteRenderer>();
            sr.sortingOrder = g.Y+1;
            base.OnFinishMove();
        }

        public override Vector3Int FindWay()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var igo = m.Field.FindGrid("Bunker.Game.GemTile");
            if (igo != null)
            {
                var wp = igo.Node.transform.position;
                //首先确定一个方向
                var dx = wp.x - transform.position.x;
                var dy = wp.y - transform.position.y;
                int startIdx = -1;
                if (Mathf.Abs(dx) > Mathf.Abs(dy))
                {
                    if (dx > 0) startIdx = 2;
                    else startIdx = 3;
                }
                else
                {
                    if (dy > 0) startIdx = 0;
                    else startIdx = 1;
                }
                //循环判断是否可行
                for (int i = 0;i< DirList.Length;++i)
                {
                    var idx = startIdx + i;
                    if (idx >= DirList.Length) idx -= DirList.Length;
                    if (m.Field.CanWalk(_curNode.X + DirList[idx].x , _curNode.Y - DirList[idx].y))
                    {
                        return DirList[idx];
                    }
                }

            }
            return Vector3Int.zero;
        }
    }
}
